
using UnityEngine;

namespace Dave6.GameStateFlow
{
    public interface IInteractor
    {
        Transform origin { get; }
        void ClearInteractable();
    }

}
