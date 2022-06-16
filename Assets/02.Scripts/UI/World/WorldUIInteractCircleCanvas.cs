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

        // ��ġ ����
        public void Show(Transform parent, Vector3 localPosition)
        {
            // �ִϸ��̼� ����????

            rectTransform.SetParent(parent);
            transform.localPosition = localPosition;
            canvas.enabled = true;           
        }

        // �θ�� ���� ��ġ��
        public void Hide()
        {
            canvas.enabled = false;
            rectTransform.SetParent(originParent);
        }
    }
}
