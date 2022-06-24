using UnityEngine;

namespace lsy
{

    public abstract class InteractBase : MonoBehaviour
    {
        [field: SerializeField]
        public string InteractObjResourceName { get; protected set; }

        public virtual void Interact()
        {
            PlayerController.Instance.CheckInteract(this, transform);
        }

        public abstract Sprite LoadButtonImage();
    }
}
