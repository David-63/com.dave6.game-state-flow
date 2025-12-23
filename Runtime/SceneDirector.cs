using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Dave6.GameStateFlow
{
    public class SceneDirector : MonoBehaviour
    {
        public static SceneDirector instance { get; private set; }
        public event UnityAction<string> onSceneFullyEntered;
        string m_NextSpawnId;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void RequestSceneLoad(string name, string spawnId = null)
        {
            m_NextSpawnId = spawnId;
            GameFlowController.instance.ChangeState(eGameState.Loading);
            StartCoroutine(LoadSceneCoroutine(name));
        }

        IEnumerator LoadSceneCoroutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[SceneDirector] Scene loaded: {scene.name}");

            // 스폰 완료 후 알림 (지연 보장)
            PlayerSpawner.instance.SpawnPlayer(m_NextSpawnId, () =>
            {
                onSceneFullyEntered?.Invoke(scene.name);
            });
        }
    }

}
