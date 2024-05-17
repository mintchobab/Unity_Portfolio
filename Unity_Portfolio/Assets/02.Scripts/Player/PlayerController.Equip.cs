using System;
using UnityEngine;

namespace lsy
{
    public partial class PlayerController
    {
        [SerializeField]
        private Transform leftHand;

        [SerializeField]
        private Transform rightHand;

        private GameObject currentTool;
        private GameObject currentWeapon;
        private GameObject currentShield;

        // TODO
        //private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        public void Equip(Action onEquiped = null)
        {
            //EquipItem weapon = equipInventoryManager.EquipedItemDic[EquipType.Weapon];
            //EquipItem shield = equipInventoryManager.EquipedItemDic[EquipType.Shield];

            //bool isWeapon = EquipWeapon(weapon);
            //bool isShield = EquipShield(shield);

            //if (isWeapon && isShield)
            //{
            //    onEquiped?.Invoke();
            //}
        }


        public void UnEquip()
        {
            UnEquipWeapon();
            UnEquipShield();
        }


        // TODO
        //public bool EquipWeapon(EquipItem equipItem)
        //{
        //    if (equipItem == null)
        //    {
        //        Managers.Instance.UIManager.SystemUIController.ShowSystemText("Text_EquipWeapon");
        //        return false;
        //    }

        //    if (currentWeapon != null)
        //        UnEquipWeapon();

        //    string path = $"{ResourcePath.Equip}/{equipItem._resourceName}";
        //    GameObject weapon = Managers.Instance.ResourceManager.Instantiate<GameObject>(path, rightHand);

        //    weapon.transform.localPosition = Vector3.zero;
        //    weapon.transform.localRotation = Quaternion.identity;

        //    currentWeapon = weapon;

        //    return true;
        //}


        public void UnEquipWeapon()
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }


        // TODO
        //public bool EquipShield(EquipItem equipItem)
        //{
        //    if (equipItem == null)
        //    {
        //        Managers.Instance.UIManager.SystemUIController.ShowSystemText("Text_EquipShield");
        //        return false;
        //    }

        //    if (currentShield != null)
        //        UnEquipShield();

        //    string path = $"{ResourcePath.Equip}/{equipItem._resourceName}";
        //    GameObject shield = Managers.Instance.ResourceManager.Instantiate<GameObject>(path, leftHand);

        //    shield.transform.localPosition = Vector3.zero;
        //    shield.transform.localRotation = Quaternion.identity;

        //    currentShield = shield;

        //    return true;
        //}


        public void UnEquipShield()
        {
            Destroy(currentShield);
            currentShield = null;
        }


        private void ShowCurrentEquip()
        {
            currentWeapon?.SetActive(true);
            currentShield?.SetActive(true);
        }


        private void HideCurrentEquip()
        {
            currentWeapon?.SetActive(false);
            currentShield?.SetActive(false);
        }



        public void MakeTool(InteractCollectionType collectionType)
        {
            switch (collectionType)
            {
                case InteractCollectionType.Mining:
                    currentTool = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.Pickax, rightHand);
                    break;

                case InteractCollectionType.Fishing:
                    currentTool = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.FishingRod, leftHand);
                    break;
            }

            HideCurrentEquip();
        }


        public void DestoryCurrentTool()
        {
            Destroy(currentTool);
            ShowCurrentEquip();
        }
    }
}