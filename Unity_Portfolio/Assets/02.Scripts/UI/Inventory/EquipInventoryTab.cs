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
            // TODO
            //Managers.Instance.EquipInventoryManager.SortInventory(tabType);
        }

    }
}