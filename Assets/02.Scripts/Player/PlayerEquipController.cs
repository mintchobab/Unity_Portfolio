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


        #region ���

        //public void Equip(EquipType equipType, EquipItem equipItem)
        public void Equip()
        {
            // ����� ��� �Ŵ������� ����� ã�ƿ���
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


        // �����տ� ���� ����
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


        // ���� ����
        public void UnEquipWeapon()
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }


        // �޼տ� ���� ����
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


        // ���� ����
        public void UnEquipShield()
        {
            Destroy(currentShield);
            currentShield = null;
        }


        // ���� ���� ���� Ȱ��ȭ
        private void ShowCurrentEquip()
        {
            currentWeapon?.SetActive(true);
            currentShield?.SetActive(true);
        }


        // ���� ���� ���� ��Ȱ��ȭ
        private void HideCurrentEquip()
        {
            currentWeapon?.SetActive(false);
            currentShield?.SetActive(false);
        }

        #endregion


        #region ����

        // ���� ����
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
