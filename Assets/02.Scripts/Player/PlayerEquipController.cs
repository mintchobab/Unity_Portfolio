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


        #region 장비

        public void Equip(EquipType equipType, EquipItem equipItem)
        {
            switch (equipType)
            {
                case EquipType.Weapon:
                    EquipWeapon(equipItem);
                    break;

                case EquipType.Shield:
                    EquipShield(equipItem);
                    break;

                default:
                    return;
            }
        }


        public void UnEquip(EquipType equipType)
        {
            switch (equipType)
            {
                case EquipType.Weapon:
                    UnEquipWeapon();
                    break;

                case EquipType.Shield:
                    UnEquipShield();
                    break;

                default:
                    return;
            }
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

        #endregion


        #region 도구

        // 도구 생성
        public void MakeTool(InteractType interactType, bool isLeft)
        {
            Transform parent = isLeft ? leftHand : rightHand;

            switch (interactType)
            {
                case InteractType.Pickaxing:
                    currentTool = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.Pickax, parent);
                    //currentTool.transform.localPosition = new Vector3(0, 0, 0);
                    //currentTool.transform.localRotation = Quaternion.identity;
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
