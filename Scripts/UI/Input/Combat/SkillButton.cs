using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace lsy
{
    public class SkillButton : CombatButton
    {
        [SerializeField]
        private Image coolDownImage;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Vector2 movePosition;


        private Vector2 originPosition;


        protected override void Awake()
        {
            base.Awake();

            originPosition = rectTransform.anchoredPosition;
            rectTransform.localScale = new Vector2(buttonTransitionScale, buttonTransitionScale);
        }

        private void OnDisable()
        {
            SkillButton.image.color = Color.white;
            coolDownImage.gameObject.SetActive(false);
        }


        public override void SetButton(Sprite sprite, string name = null)
        {
            if (name.Equals(string.Empty))
                name = "Empty Skill";

            nameText.text = name;
            SkillButton.image.sprite = sprite;
            coolDownImage.sprite = sprite;
        }


        public IEnumerator FillImageCoolTime(float coolTime)
        {
            float t = 0f;

            Color startColor = SkillButton.image.color;
            Color tmpColor = startColor;
            tmpColor.a = 80f / 255f;
            SkillButton.image.color = tmpColor;

            coolDownImage.gameObject.SetActive(true);

            while (t < 1)
            {
                t += Time.deltaTime / coolTime;
                coolDownImage.fillAmount = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }

            SkillButton.image.color = startColor;
            coolDownImage.gameObject.SetActive(false);
        }


        public void MoveAndSizeUp()
        {
            rectTransform.DOAnchorPos(movePosition, buttonTransitionTime);
            rectTransform.DOScale(1f, buttonTransitionTime);
        }


        public void MoveAndSizeDown()
        {
            rectTransform.DOAnchorPos(originPosition, buttonTransitionTime);
            rectTransform.DOScale(buttonTransitionScale, buttonTransitionTime);
        }

    }
}
