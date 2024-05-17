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


        public void Show(InventoryManager.InventoryItem inventoryItem, int slotIndex)
        {
            currentSlotIndex = slotIndex;

            itemImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>(Tables.ItemTable[inventoryItem.ItemId].SpritePath);
            itemName.text = StringManager.Get(Tables.ItemTable[inventoryItem.ItemId].Name);

            itemDescription.text = StringManager.Get(Tables.ItemTable[inventoryItem.ItemId].Explanation);
            gameObject.SetActive(true);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.pointerCurrentRaycast.gameObject.Equals(gameObject))
                return;

            gameObject.SetActive(false);
        }


        private void OnClickUseButton()
        {
            inventoryManager.UseItem(currentSlotIndex);
            gameObject.SetActive(false);
        }

    }
}
