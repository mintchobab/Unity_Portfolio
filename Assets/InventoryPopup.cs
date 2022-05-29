using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace lsy
{
    public class InventoryPopup : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private Text itemName;

        [SerializeField]
        private Text itemDescription;

        [SerializeField]
        private Button useButton;


        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;

        private int currentSlotIndex;


        private void Awake()
        {
            useButton.onClick.AddListener(OnClickUseButton);
        }


        public void Show(CountableItem item, int slotIndex)
        {
            currentSlotIndex = slotIndex;

            itemImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>($"{ResourcePath.ConsumableItem}/{item.uniqueName}");
            itemName.text = StringManager.GetLocalizedItemName(item.name);
            itemDescription.text = StringManager.GetLocalizedItemDescription(item.description);

            gameObject.SetActive(true);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }


        // ������ ���
        private void OnClickUseButton()
        {
            inventoryManager.UseItem(currentSlotIndex);
            gameObject.SetActive(false);
        }

    }
}
