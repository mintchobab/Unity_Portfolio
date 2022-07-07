using UnityEngine;

namespace lsy
{
    public class PlayerEquipController : MonoBehaviour
    {
        [SerializeField]
        private Transform leftHand;

        [SerializeField]
        private Transform rightHand;

        private GameObject currentTool;
        private GameObject currentWeapon;
        private GameObject currentShield;

        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        #region 장비

        //public void Equip(EquipType equipType, EquipItem equipItem)
        public void Equip()
        {
            // 무기랑 장비를 매니저에서 어떤건지 찾아오기
            EquipItem weapon = equipInventoryManager.EquipedItemDic[EquipType.Weapon];
            EquipItem shield = equipInventoryManager.EquipedItemDic[EquipType.Shield];

            EquipWeapon(weapon);
            EquipShield(shield);
        }


        public void UnEquip()
        {
            UnEquipWeapon();
            UnEquipShield();
        }


        // 오른손에 무기 장착
        public void EquipWeapon(EquipItem equipItem)
        {
            if (currentWeapon != null)
                UnEquipWeapon();

            string path = $"{ResourcePath.Equip}/{equipItem._resourceName}";
            GameObject weapon = Managers.Instance.ResourceManager.Instantiate<GameObject>(path, rightHand);

            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;

            currentWeapon = weapon;
        }


        // 무기 해제
        public void UnEquipWeapon()
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }


        // 왼손에 방패 장착
        public void EquipShield(EquipItem equipItem)
        {
            if (currentShield != null)
                UnEquipShield();

            string path = $"{ResourcePath.Equip}/{equipItem._resourceName}";
            GameObject shield = Managers.Instance.ResourceManager.Instantiate<GameObject>(path, leftHand);

            shield.transform.localPosition = Vector3.zero;
            shield.transform.localRotation = Quaternion.identity;

            currentShield = shield;
        }


        // 방패 해제
        public void UnEquipShield()
        {
            Destroy(currentShield);
            currentShield = null;
        }


        // 현재 장착 무기 활성화
        private void ShowCurrentEquip()
        {
            currentWeapon?.SetActive(true);
            currentShield?.SetActive(true);
        }


        // 현재 장착 무기 비활성화
        private void HideCurrentEquip()
        {
            currentWeapon?.SetActive(false);
            currentShield?.SetActive(false);
        }

        #endregion


        #region 도구

        // 도구 생성
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

        #endregion
    }
}
