using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace lsy
{
    public class TestScript : MonoBehaviour
    {
        private void Start()
        {
            Managers.Instance.QuestManager.SetQuestToNPC(2000);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Managers.Instance.InventoryManager.AddCountableItem(100, 3);
            }
            // 아이템 생성 1
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(0);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(10);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(20);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(30);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(40);
            }

            // 아이템 생성 2
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(11);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(21);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(31);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                Managers.Instance.EquipInventoryManager.AddEquipItem(41);
            }



            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Managers.Instance.EquipInventoryManager.RemoveEquipItem(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Managers.Instance.EquipInventoryManager.RemoveEquipItem(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Managers.Instance.EquipInventoryManager.RemoveEquipItem(2);
            }
        }
    }
}
