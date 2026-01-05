using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityUtils;

namespace Dave6.GameStateFlow
{
    public enum eGameState
    {
        Boot,       // 시작 직후
        Stopped,    // 시작메뉴 조작가능
        Loading,    // 씬 준비
        Running,    // 플레이 중
        Paused,     // 일시정지
    }


    public class GameFlowController : SingletonTemplate<GameFlowController>
    {
        public event UnityAction<eGameState, eGameState> onStateChanged;
        [SerializeField] eGameState m_CurrentState = eGameState.Boot;
        public eGameState gameState => m_CurrentState;

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        void Start()
        {
            SceneDirector.instance.RequestSceneLoad("Lobby", "LobbyEnter");
        }
        public void ChangeState(eGameState newState)
        {
            if (m_CurrentState == newState) return;

            var previous = m_CurrentState;
            m_CurrentState = newState;

            Debug.Log($"[GameFlow] State: {previous} → {newState}");
            onStateChanged?.Invoke(previous, newState);
        }

        #region 커서 및 상태 관리
        public void CursorLock() => Cursor.lockState = CursorLockMode.Locked;
        public void CursorUnlock() => Cursor.lockState = CursorLockMode.None;

        public void PauseGame()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void ResumeGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        #endregion

    }

}
