using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class QuestUIProgressBar : MonoBehaviour
    {
        [SerializeField]
        private Text questGoalText;

        [SerializeField]
        private Image[] barImages;

        private List<Image> activatedBarImages = new List<Image>();
        private RectTransform rectTransform;

        private QuestManager questManager => Managers.Instance.QuestManager;

        private Vector2 barStartPosition = new Vector2(0f, 80f);
        private Vector2 barEndPosition = Vector2.zero;
        

        private readonly float barColorChangeTime = 1f;
        private readonly float barShowTime = 1f;
        private readonly float barDelay = 2f;

        private readonly string barImageColor = "#FF8019";
        private string questGoalString;


        private void OnDisable()
        {
            rectTransform.anchoredPosition = barStartPosition;
        }


        public void Initialize()
        {
            rectTransform = GetComponent<RectTransform>();

            questManager.onQuestChanged += OnQuestChanged;
            questManager.onChangedCurrentQuestCount += OnChangedCurrentQuestCount;
        }


        private void OnQuestChanged()
        {
            if (questManager.CurrentQuest == null)
                return;

            int goalCount = questManager.CurrentQuest.GoalCount;

            if (barImages.Length < goalCount)
            {
                Debug.LogError($"{nameof(QuestUIProgressBar)} : Count Error");
                return;
            }

            int currentCount = questManager.CurrentQuest.CurrentCount;
            questGoalString = StringManager.Get(questManager.CurrentQuest.GetQuestTableData().Goal);

            float barWidth = rectTransform.rect.width / goalCount;
            float xPos = 0f;

            for (int i = 0; i < barImages.Length; i++)
            {
                if (i < goalCount)
                {
                    barImages[i].rectTransform.sizeDelta = new Vector2(barWidth, barImages[i].rectTransform.sizeDelta.y);
                    barImages[i].rectTransform.anchoredPosition = new Vector2(xPos, 0);
                    xPos += barWidth;

                    if (i < currentCount)
                    {
                        ColorUtility.TryParseHtmlString(barImageColor, out Color targetColor);
                        barImages[i].color = targetColor;
                    }
                    else
                    {
                        barImages[i].color = Color.black;
                    }

                    barImages[i].gameObject.SetActive(true);
                }
                else
                {
                    barImages[i].rectTransform.sizeDelta = Vector2.zero;
                    barImages[i].rectTransform.anchoredPosition = Vector2.zero;
                    barImages[i].color = Color.white;

                    barImages[i].gameObject.SetActive(false);
                }
            }
        }


        private void OnChangedCurrentQuestCount()
        {
            int currentCount = questManager.CurrentQuest.CurrentCount;

            questGoalText.text = questGoalString + $" ({currentCount}/{questManager.CurrentQuest.GoalCount})";

            StartCoroutine(ChangePosition());

            if (currentCount <= questManager.CurrentQuest.GoalCount)
            {
                barImages[currentCount - 1].gameObject.SetActive(true);
                StartCoroutine(ChangeBarColor(barImages[currentCount - 1]));
            }           
        }


        private IEnumerator ChangePosition()
        {
            float time = 0f;
            float showTime = barShowTime;

            Vector2 startPosition = barStartPosition;
            Vector2 targetPosition = barEndPosition;

            while (time < 1f)
            {
                time += Time.deltaTime / showTime;
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time);
                yield return null;
            }

            yield return new WaitForSeconds(barDelay);

            time = 0f;
            Vector2 tmp = startPosition;
            startPosition = targetPosition;
            targetPosition = tmp;

            while(time < 1f)
            {
                time += Time.deltaTime / showTime;
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time);
                yield return null;
            }
        }


        private IEnumerator ChangeBarColor(Image barImage)
        {
            float time = 0f;
            float targetTime = barColorChangeTime;

            ColorUtility.TryParseHtmlString(barImageColor, out Color targetColor);

            yield return new WaitForSeconds(barShowTime * 0.5f);

            barImage.color = Color.white;
            while (time < 1f)
            {
                time += Time.deltaTime / targetTime;
                barImage.color = Color.Lerp(Color.white, targetColor, time);
                yield return null;
            }
        }
    }
}
