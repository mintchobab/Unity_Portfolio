using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace lsy 
{
    public class TabController : MonoBehaviour
    {
        [field: SerializeField]
        public Sprite DefaultImage { get; private set; }

        [field: SerializeField]
        public Sprite SelectedImage { get; private set; }

        [SerializeField]
        private List<TabButton> tabButtons = new List<TabButton>();


        private void Awake()
        {
            foreach(TabButton button in tabButtons)
            {
                button.Initialized(this);
            }
        }

        private void OnEnable()
        {
            tabButtons[0].Selected();

            if (tabButtons.Count > 2)
            {
                for (int i = 1; i < tabButtons.Count; i++)
                {
                    tabButtons[i].UnSelected();
                }
            }            
        }


        public void ClickedTabButton(TabButton tabButton)
        {
            // 선택 버튼 이외 나머지 Unelect;
            foreach(TabButton button in tabButtons)
            {
                if (button != tabButton)
                {
                    button.UnSelected();
                }
            }
        }

    }
}



