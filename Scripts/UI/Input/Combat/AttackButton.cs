using UnityEngine;
using DG.Tweening;

namespace lsy
{
    public class AttackButton : CombatButton
    {
        public override void SetButton(Sprite sprite, string name = null)
        {
            SkillButton.image.sprite = sprite;
        }


        public void SizeUp()
        {
            rectTransform.DOScale(1f, buttonTransitionTime);
        }


        public void SizeDown()
        {
            rectTransform.DOScale(0f, buttonTransitionTime);
        }
    }
}
