using System.Collections;
using System.Collections.Generic;
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
    }


    public class InteractCollection : MonoBehaviour, IInteractable, ITargetable
    {
        [field: SerializeField]
        public InteractCollectionType CollectionType { get; private set; }

        [field: SerializeField]
        public int ItemId { get; private set; }

        public CollectionData MyCollectionData { get; private set; }


        private void Awake()
        {
            SetCollectionData();
        }


        public void Interact()
        {
            PlayerController.Instance.StartInteract(this, transform);
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


        //public override Sprite LoadButtonImage()
        //{
        //    return Managers.Instance.ResourceManager.Load<Sprite>(MyCollectionData.ButtonIconPath);
        //}


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
            collectionData.InteractTime = ValueData.MiningTime;
            collectionData.InteractDistance = ValueData.MiningDistance;
            collectionData.ButtonIconPath = ResourcePath.IconMining;
            collectionData.gaugePosition = new Vector3(0f, 1f, 0f);
            collectionData.itemType = ItemType.Material;

            return collectionData;
        }

                

        private CollectionData GetSearchData()
        {
            CollectionData collectionData = new CollectionData();
            collectionData.AnimationHash = Animator.StringToHash("search");
            collectionData.InteractTime = ValueData.SearchTime;
            collectionData.InteractDistance = ValueData.SearchDistance;
            collectionData.ButtonIconPath = ResourcePath.IconSearch;
            collectionData.gaugePosition = new Vector3(0f, 1f, 0f);
            collectionData.itemType = ItemType.Material;

            return collectionData;
        }


        private CollectionData GetFisingData()
        {
            CollectionData collectionData = new CollectionData();
            collectionData.AnimationHash = Animator.StringToHash("fishing");
            collectionData.InteractTime = ValueData.FishingTime;
            collectionData.InteractDistance = ValueData.FishingDistance;
            collectionData.ButtonIconPath = ResourcePath.IconFishing;
            collectionData.gaugePosition = new Vector3(0f, 1f, 0f);
            collectionData.itemType = ItemType.Material;
            return collectionData;
        }
    }
}
