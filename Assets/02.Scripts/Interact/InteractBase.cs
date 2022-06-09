using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public struct InteractData
    {
        public InteractType InteractType;

        public int StartHash;
        public int EndHash;
        public float InteractTime;
        public float InteractDistance;
        public string ButtonIconPath;
    }


    public abstract class InteractBase : MonoBehaviour
    {
        [SerializeField]
        protected string interactObjNameKey;

        protected PlayerController playerController;
        protected InteractData interactData;


        protected BillboardUIController billboardUIController => Managers.Instance.UIManager.BillboardUIController;


        protected virtual void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            SetInteractData();
        }


        public Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(interactData.ButtonIconPath);
        }


        public virtual void Interact()
        {
            playerController.StartInteract(interactData, transform);
        }


        protected abstract void SetInteractData();
    }
}
