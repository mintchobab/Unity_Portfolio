using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace lsy
{
    public class AttackButton : MonoBehaviour
    {
        private Button button;
        private RectTransform rectTransform;
        private CombatButtonController combatButtonController;


        private void Awake()
        {
            button = GetComponent<Button>();
            rectTransform = GetComponent<RectTransform>();
            combatButtonController = GetComponentInParent<CombatButtonController>();

            button.onClick.AddListener(OnClickButton);
        }


        private void OnClickButton()
        {
            combatButtonController.TestButtonMove();
            rectTransform.DOScale(0f, 0.35f);
        }
    }
}
