using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;

    public ItemData ItemData { get => itemData; }


    [SerializeField]
    private int itemCount = 1;
    public int ItemCount { get => itemCount; }
}
