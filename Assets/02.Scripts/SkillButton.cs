using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace lsy
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Image coolDownImage;

        [SerializeField]
        private Text nameText;


        [SerializeField]
        private Vector2 movePosition;

        private RectTransform rectTransform;



        private void Awake()
        {
            button = GetComponent<Button>();
            rectTransform = GetComponent<RectTransform>();

            button.onClick.AddListener(() =>
            {
                StartCoroutine(FillImageCoolTime(5f));
            });
        }


        public void ChangeButton(Sprite sprite, string name)
        {
            // 버튼 클릭 이벤트도 변경하기
            nameText.text = name;
            button.image.sprite = sprite;
            coolDownImage.sprite = sprite;
        }



        public IEnumerator FillImageCoolTime(float coolTime)
        {
            float t = 0f;

            Color startColor = button.image.color;
            Color tmpColor = startColor;
            tmpColor.a = 80f / 255f;
            button.image.color = tmpColor;

            coolDownImage.gameObject.SetActive(true);

            while (t < 1)
            {
                t += Time.deltaTime / coolTime;
                coolDownImage.fillAmount = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }

            button.image.color = startColor;
            coolDownImage.gameObject.SetActive(false);
        }


        public void TestMove()
        {
            rectTransform.DOAnchorPos(movePosition, ValueData.SkillButtonTransitionTime);
            rectTransform.DOScale(1f, ValueData.SkillButtonTransitionTime);
        }

    }
}
