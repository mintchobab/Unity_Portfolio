using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class EquipInventoryTab : MonoBehaviour
    {
        [SerializeField]
        private EquipType tabType;

        private Button button;


        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickButton);
        }


        private void OnClickButton()
        {
            Managers.Instance.EquipInventoryManager.SortInventory(tabType);
        }

    }
}