using System;
using System.Collections;
using UnityEngine;
using UnityUtils;

namespace Dave6.GameStateFlow
{
    public class PlayerSpawner : SingletonTemplate<PlayerSpawner>
    {
        [SerializeField] GameObject m_PlayerPrefab;
        GameObject m_PlayerInstance;
        IEntity m_Player;

        void Start()
        {
            if (m_PlayerInstance == null && m_PlayerPrefab != null)
            {
                m_PlayerInstance = Instantiate(m_PlayerPrefab);
                m_Player = m_PlayerInstance.GetComponent<IEntity>();                
                DontDestroyOnLoad(m_PlayerInstance);

                // 로딩 전까진 비활성화
                m_PlayerInstance.SetActive(false);
            }
            DontDestroyOnLoad(Camera.main);
        }

        public void SpawnPlayer(string spawnId, Action onComplete = null)
        {
            Portal targetPortal = null;

            var portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);
            foreach (var portal in portals)
            {
                if (portal.portalId == spawnId)
                {
                    targetPortal = portal;
                    break;
                }
            }

            if (m_PlayerInstance == null && m_PlayerPrefab != null)
            {
                m_PlayerInstance = Instantiate(m_PlayerPrefab);
                DontDestroyOnLoad(m_PlayerInstance);
            }

            if (targetPortal != null)
            {
                m_PlayerInstance.transform.position = targetPortal.transform.position;
                m_PlayerInstance.transform.rotation = targetPortal.transform.rotation;

                var cc = m_PlayerInstance.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                    cc.enabled = true;
                }

                Debug.Log($"[PlayerSpawner] Player spawned at: {targetPortal.name}");
            }
            else
            {
                Debug.LogWarning($"[PlayerSpawner] Portal ID '{spawnId}' not found. Using current position.");
            }

            m_PlayerInstance.SetActive(true);

            // 한 프레임 지연 후 게임 시작 (타이밍 안정화)
            StartCoroutine(DelayedResume(onComplete));
        }

        IEnumerator DelayedResume(Action onComplete)
        {
            yield return new WaitForEndOfFrame();
            GameFlowController.instance.ChangeState(eGameState.Running);
            onComplete?.Invoke();
        }
    }

}
