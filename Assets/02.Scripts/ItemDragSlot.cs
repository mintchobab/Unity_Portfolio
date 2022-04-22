using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragSlot : MonoBehaviour
{
    //[SerializeField]
    //private float alpha = 0;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void StartDrag(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
