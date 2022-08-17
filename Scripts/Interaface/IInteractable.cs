using UnityEngine;

namespace lsy
{
    public interface IInteractable
    {
        public void Interact();
        public float GetInteractDistance();
        public Transform GetTransform();
        public Sprite LoadButtonImage();
    }
}
