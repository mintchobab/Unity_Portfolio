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

            float width = 0f;

#if UNITY_EDITOR
            width = transform.parent.GetComponent<RectTransform>().rect.width;
#elif UNITY_ANDROID
            width = (rootCanvas.renderingDisplaySize.x);
#endif

            width = (width - 1920f) * 0.5f - 2f;
            width = Mathf.Max(width, 0);

            myRect.sizeDelta = new Vector2(width, myRect.rect.height);
        }
    }
}
