using UnityEngine;

namespace lsy
{

    public abstract class InteractBase : MonoBehaviour
    {
        [field: SerializeField]
        public string InteractObjResourceName { get; protected set; }

        public virtual void Interact()
        {
        }

        public abstract Sprite LoadButtonImage();
    }
}
