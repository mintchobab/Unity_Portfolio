using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTabButton : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultImage;

    [SerializeField]
    private Sprite selectedImage;

    private Image image;


    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Selected()
    {
        image.sprite = selectedImage;
    }

    public void UnSelected()
    {
        image.sprite = defaultImage;
    }
}
