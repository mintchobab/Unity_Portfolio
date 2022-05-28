using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public abstract class InteractBase : MonoBehaviour
    {
        [field: SerializeField]
        public InteractType InteractType;

        protected Sprite buttonImage;


        public abstract void Interact();
        protected abstract void SetButtonImage();


        protected virtual void Awake()
        {
            SetButtonImage();
        }

    }
}
