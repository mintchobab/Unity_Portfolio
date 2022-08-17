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

            Quest quest = questManager.CurrentQuest.Quest;
            int count = 0;

            if (quest.task.collect.Count != 0)
            {
                count = quest.task.collect[1];
            }
            else if (quest.task.kill.Count != 0)
            {
                count = quest.task.kill[1];
            }

            questGoalString = StringManager.GetLocalizedQuestGoal(quest.goal);

            ClearBarImages();
            ActivateBarImages(count);
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


        private void ActivateBarImages(int imageCount)
        {
            float barWidth = rectTransform.rect.width / questManager.CurrentQuest.GoalCount;
            float xPos = 0f;

            for (int i = 0; i < imageCount; i++)
            {
                barImages[i].rectTransform.sizeDelta = new Vector2(barWidth, barImages[i].rectTransform.sizeDelta.y);
                barImages[i].rectTransform.anchoredPosition = new Vector2(xPos, 0);
                xPos += barWidth;
                activatedBarImages.Add(barImages[i]);
            }
        }


        private void ClearBarImages()
        {
            if (activatedBarImages.Count != 0)
            {
                for (int i = 0; i < activatedBarImages.Count; i++)
                {
                    activatedBarImages[i].color = Color.white;
                    activatedBarImages[i].gameObject.SetActive(false);
                }
            }

            activatedBarImages.Clear();
        }

    }
}

