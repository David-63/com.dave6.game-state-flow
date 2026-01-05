using UnityEngine;
namespace Dave6.GameStateFlow
{
    /*
    1 포탈 연출
    화면 페이드
    입력 잠금
    이동 불가 처리

    2 포탈 조건
    퀘스트 완료 여부
    키 아이템
    난이도 제한

    3 씬 내부 포탈
    같은 씬, 위치만 이동
    씬 로드 없는 워프
    */
    public class Portal : MonoBehaviour, IInteractable
    {
        [SerializeField] string m_TargetScene;
        [SerializeField] string m_PortalId;
        public string portalId => m_PortalId;
        [SerializeField] string m_ConnectId;
        public string connectId => m_ConnectId;

        bool isCunsumed = false;

        public void Interact(IInteractor interactor)
        {
            if (isCunsumed) return;
            isCunsumed = true;

            SceneDirector.instance.RequestSceneLoad(m_TargetScene, m_ConnectId);
        }
    }

}
