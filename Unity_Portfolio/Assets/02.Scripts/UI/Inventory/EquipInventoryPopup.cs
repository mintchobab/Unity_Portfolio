using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace lsy
{
    public class EquipInventoryPopup : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private Text itemName;

        [SerializeField]
        private Text itemDescription;

        [SerializeField]
        private Text powerText;

        [SerializeField]
        private Text hpText;

        [SerializeField]
        private Text mpText;

        [SerializeField]
        private Button equipButton;
        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;

        private int currentSlotIndex;


        // TODO : 매개변수 확인
        public void Show(bool isInventory, int itemId, int slotIndex = -1)
        {
            if (slotIndex != -1)
                currentSlotIndex = slotIndex;

            EquipmentItemTable.TableData tableData = Tables.EquipmentItemTable[itemId];

            itemImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>(tableData.SpritePath);
            itemName.text = StringManager.Get(tableData.Name);
            itemDescription.text = StringManager.Get(tableData.Explanation);

            powerText.text = tableData.Hp.ToString();
            hpText.text = tableData.OffensivePower.ToString();
            mpText.text = tableData.DefensivePower.ToString();

            if (isInventory)
            {
                equipButton.GetComponentInChildren<Text>().text = StringManager.Get("Text_Equip");
                equipButton.onClick.SetListener(OnClickEquipButton);
            }
            else
            {
                equipButton.GetComponentInChildren<Text>().text = StringManager.Get("Text_UnEquip");
                equipButton.onClick.SetListener(() => OnClickUnequipButton(itemId));
            }

            gameObject.SetActive(true);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }


        private void OnClickEquipButton()
        {
            // TODO
            inventoryManager.Equip(currentSlotIndex);
            gameObject.SetActive(false);
        }

        private void OnClickUnequipButton(int itemId)
        {
            inventoryManager.UnEquip(itemId);
            gameObject.SetActive(false);
        }
    }
}
