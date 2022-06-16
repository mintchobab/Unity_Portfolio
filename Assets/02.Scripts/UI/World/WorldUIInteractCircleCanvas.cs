using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class WorldUIInteractCircleCanvas : MonoBehaviour
    {
        private Canvas canvas;

        private Transform originParent;
        private RectTransform rectTransform;

        public void Initialize(Transform parent)
        {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;


            rectTransform = GetComponent<RectTransform>();
            originParent = parent;
            Hide();
        }

        // 위치 변경
        public void Show(Transform parent, Vector3 localPosition)
        {
            // 애니메이션 실행????

            rectTransform.SetParent(parent);
            transform.localPosition = localPosition;
            canvas.enabled = true;           
        }

        // 부모는 원래 위치로
        public void Hide()
        {
            canvas.enabled = false;
            rectTransform.SetParent(originParent);
        }
    }
}
