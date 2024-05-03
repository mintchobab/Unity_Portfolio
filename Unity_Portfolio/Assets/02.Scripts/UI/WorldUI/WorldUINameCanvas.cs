using UnityEngine;
using UnityEngine.UI;

namespace lsy 
{
    public class WorldUINameCanvas : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;

        private Canvas canvas;
        private Camera cam;
        private RectTransform rect;


        private void Awake()
        {
            cam = Camera.main;

            rect = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = cam;
        }


        public void Show()
        {
            canvas.enabled = true;
        }


        public void Hide()
        {
            canvas.enabled = false;
        }


        public void SetName(string name)
        {
            nameText.text = name;
        }


        public void SetNameColor(Color nameColor)
        {
            nameText.color = nameColor;
        }


        public void SetParent(Transform parent)
        {
            rect.SetParent(parent);
        }


        public void SetLocalPosition(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }

    }
}
