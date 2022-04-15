using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemCatcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ItemObject itemObj = other.GetComponent<ItemObject>();

        if (itemObj)
        {
            InventorySystem.Instance.Add(itemObj.ItemData, itemObj.ItemCount);
        }
    }
}
