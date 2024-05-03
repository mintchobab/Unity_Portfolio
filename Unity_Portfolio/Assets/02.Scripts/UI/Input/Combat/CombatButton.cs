using System;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public abstract class CombatButton : MonoBehaviour
    {
        [field: SerializeField]
        public Button SkillButton { get; private set; }

        protected RectTransform rectTransform;

        protected Action onClickButton;

        protected readonly float buttonTransitionTime = 0.28f;
        protected readonly float buttonTransitionScale = 0.8f;


        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public abstract void SetButton(Sprite sprite, string name = null);
    }
}
