using System.Collections;
using System.Collections.Generic;
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

        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;

        private int currentSlotIndex;


        public void Show(EquipItem item, bool isInventory, int slotIndex = -1)
        {
            if (slotIndex != -1)
                currentSlotIndex = slotIndex;

            itemImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>($"{ResourcePath.EquipItem}/{item._resourceName}");
            itemName.text = StringManager.GetLocalizedItemName(item.name);
            itemDescription.text = StringManager.GetLocalizedItemExplanation(item.explanation);

            powerText.text = item.hp.ToString();
            hpText.text = item.offensivePower.ToString();
            mpText.text = item.defensivePower.ToString();

            // 장착 / 해제 버튼 구분
            if (isInventory)
            {
                equipButton.GetComponentInChildren<Text>().text = StringManager.GetLocalizedUIText("Text_Equip");
                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(OnClickEquipButton);
            }
            else
            {
                equipButton.GetComponentInChildren<Text>().text = StringManager.GetLocalizedUIText("Text_UnEquip");
                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(() => OnClickUnequipButton(item));
            }

            gameObject.SetActive(true);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            gameObject.SetActive(false);
        }


        // 아이템 장착 / 해제
        // 이벤트가 달라져야함
        private void OnClickEquipButton()
        {
            equipInventoryManager.Equip(currentSlotIndex);
            gameObject.SetActive(false);
        }

        private void OnClickUnequipButton(EquipItem item)
        {
            equipInventoryManager.UnEquip(item);
            gameObject.SetActive(false);
        }
    }
}
