using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace lsy
{
    public class TestScript : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Managers.Instance.InventoryManager.AddCountableItem(0, 3);
            }
        }
    }
}
