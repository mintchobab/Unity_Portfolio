using System;
using UnityEngine;

namespace lsy
{
    public struct CollectionData
    {
        public Vector3 gaugePosition;
        public ItemType itemType;

        public int AnimationHash;
        public float InteractTime;
        public float InteractDistance;
        public string ButtonIconPath;
        public string sfxName;
    }


    public class InteractCollection : MonoBehaviour, IInteractable, ITargetable
    {
        [field: SerializeField]
        public InteractCollectionType CollectionType { get; private set; }

        [field: SerializeField]
        public int ItemId { get; private set; }

        public Action onInteract;
        public Action onEndInteract;

        private readonly float miningTime = 3f;
        private readonly float miningDistance = 1.2f;

        private readonly float searchTime = 3f;
        private readonly float searchDistance = 1f;
        
        private readonly float fishingTime = 3f;
        private readonly float fishingDistance = 3.5f;        

        public CollectionData MyCollectionData { get; private set; }


        private void Awake()
        {
            SetCollectionData();
        }


        public void Interact()
        {
            PlayerController.Instance.StartInteract(this, transform, onInteract, onEndInteract);
        }


        public Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(MyCollectionData.ButtonIconPath);
        }


        public Transform GetTransform()
        {
            return transform;
        }


        public float GetInteractDistance()
        {
            return MyCollectionData.InteractDistance;
        }


        private void SetCollectionData()
        {
            switch (CollectionType)
            {
                case InteractCollectionType.Mining:
                    MyCollectionData = GetMiningData();
                    break;

                case InteractCollectionType.Search:
                    MyCollectionData = GetSearchData();
                    break;

                case InteractCollectionType.Fishing:
                    MyCollectionData = GetFisingData();
                    break;
            }
        }


        private CollectionData GetMiningData()
        {
            CollectionData collectionData = new CollectionData();
            collectionData.AnimationHash = Animator.StringToHash("mining");
            collectionData.InteractTime = miningTime;
            collectionData.InteractDistance = miningDistance;
            collectionData.ButtonIconPath = ResourcePath.IconMining;
            collectionData.sfxName = "Mining";
            collectionData.gaugePosition = new Vector3(0f, 1f, 0f);
            collectionData.itemType = ItemType.Material;

            return collectionData;
        }


        private CollectionData GetSearchData()
        {
            CollectionData collectionData = new CollectionData();
            collectionData.AnimationHash = Animator.StringToHash("search");
            collectionData.InteractTime = searchTime;
            collectionData.InteractDistance = searchDistance;
            collectionData.ButtonIconPath = ResourcePath.IconSearch;
            collectionData.sfxName = "Searching";
            collectionData.gaugePosition = new Vector3(0f, 1f, 0f);
            collectionData.itemType = ItemType.Material;

            return collectionData;
        }


        private CollectionData GetFisingData()
        {
            CollectionData collectionData = new CollectionData();
            collectionData.AnimationHash = Animator.StringToHash("fishing");
            collectionData.InteractTime = fishingTime;
            collectionData.InteractDistance = fishingDistance;
            collectionData.ButtonIconPath = ResourcePath.IconFishing;
            collectionData.sfxName = "Fishing";
            collectionData.gaugePosition = new Vector3(0f, 1f, 0f);
            collectionData.itemType = ItemType.Material;
            return collectionData;
        }
    }
}
