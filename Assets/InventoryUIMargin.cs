using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class InventoryUIMargin : MonoBehaviour
    {
        [SerializeField]
        private Canvas rootCanvas;

        private void Start()
        {
            RectTransform myRect = GetComponent<RectTransform>();

            //RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
            //Debug.LogWarning("Root Parent Width : " + parent.renderingDisplaySize.x);
            //Debug.LogWarning("Root Parent Width : " + parent.rect.width);
            //Debug.LogWarning("SizeDelta : " + parentRect.sizeDelta.x);

            float x = (rootCanvas.renderingDisplaySize.x - 1920f) * 0.5f - 2f;
            float y = myRect.rect.height;

            myRect.sizeDelta = new Vector2(x, y);
        }
    }
}
