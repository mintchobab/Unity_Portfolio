using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace lsy
{
    public abstract class CombatButton : MonoBehaviour
    {
        [field: SerializeField]
        public Button SkillButton { get; private set; }

        protected RectTransform rectTransform;

        protected Action onClickButton;


        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }



        public abstract void SetButton(Sprite sprite, string name = null);
    }
}
