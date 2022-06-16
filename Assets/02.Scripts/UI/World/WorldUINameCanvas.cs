using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy 
{
    public class WorldUINameCanvas : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Color nameColor;

        private Canvas canvas;
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;

            canvas = GetComponent<Canvas>();
            canvas.worldCamera = cam;
            
            nameText.color = nameColor;
        }


        public void Initialize(string name)
        {
            nameText.text = name;
            Hide();
        }

        public void Show()
        {
            canvas.enabled = true;
        }

        public void Hide()
        {
            canvas.enabled = false;
        }
    }
}
