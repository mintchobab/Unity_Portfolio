using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public struct InteractData
    {
        public InteractType InteractType;

        public ItemType ItemType;
        public int ItemId;

        public int StartHash;
        public int EndHash;        
        public float InteractTime;
        public float InteractDistance;
        public string ButtonIconPath;
    }


    public abstract class InteractBase : MonoBehaviour
    {
        [field: SerializeField]
        public string InteractObjResourceName { get; protected set; }

        public InteractData InteractData { get; protected set; }


        protected BillboardUIController billboardUIController => Managers.Instance.UIManager.BillboardUIController;


        protected virtual void Awake()
        {
            SetInteractData();
        }


        public Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(InteractData.ButtonIconPath);
        }


        public virtual void Interact()
        {
            PlayerController.Instance.StartInteract(this, transform);
        }


        protected abstract void SetInteractData();
    }
}
