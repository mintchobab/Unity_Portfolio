using UnityEngine;

namespace lsy
{
    public interface IInteractable
    {
        public void Interact();
        public Sprite LoadButtonImage();
        public Transform GetTransform();
    }
}
