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


    public class InteractCollection : InteractTargetable
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


        public override Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(MyCollectionData.ButtonIconPath);
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

    }
}


