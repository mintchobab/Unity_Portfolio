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

        private string questGoalString;

        private QuestManager questManager => Managers.Instance.QuestManager;


        private int prevCount;



        // 초기화 부분을 다시 생각해보자....................................................................
        public void Initialize()
        {
            rectTransform = GetComponent<RectTransform>();

            questManager.onQuestChanged += OnQuestChanged;
            questManager.onCurrentQuestItemCountChanged += OnCurrentQuestItemCountChanged;
        }


        // 퀘스트가 변경되면
        // 그 퀘스트에 목표 개수에 맞게 이미지가 배치됨
        private void OnQuestChanged()
        {
            if (questManager.CurrentQuest == null)
                return;

            prevCount = 0;

            Quest quest = questManager.CurrentQuest.Quest;

            // 수집 퀘스트일 때
            if (quest.task.collect.Count != 0)
            {
                questGoalString = StringManager.GetLocalizedQuestGoal(quest.goal);

                int count = quest.task.collect[1];
                ActivateBarImages(count);
            }
        }


        private void OnCurrentQuestItemCountChanged()
        {
            int currentCount = questManager.CurrentQuest.CurrentCount;

            questGoalText.text = questGoalString + $" ({currentCount}/{questManager.CurrentQuest.GoalCount})";

            // 아이템이 삭제 되는 경우
            if (currentCount < prevCount)
            {
                for (int i = barImages.Length - 1; i >= currentCount; i--)
                {
                    if (!barImages[i].gameObject.activeSelf)
                        continue;

                    barImages[i].gameObject.SetActive(false);
                }
            }
            // 아이템이 추가되는 경우
            else
            {
                barImages[currentCount - 1].gameObject.SetActive(true);
                StartCoroutine(ChangePosition());
                StartCoroutine(ChangeBarColor(barImages[currentCount - 1]));
            }

            prevCount = currentCount;
        }


        // 퀘스트 진행바 위치 변경
        private IEnumerator ChangePosition()
        {
            float time = 0f;
            float showTime = ValueData.QuestBarShowTime;

            Vector2 startPosition = new Vector2(0f, 80f);
            Vector2 targetPosition = new Vector2(0f, 0f);

            while (time < 1f)
            {
                time += Time.deltaTime / showTime;
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time);
                yield return null;
            }

            yield return new WaitForSeconds(ValueData.QuestBarDelayTime);

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


        // 퀘스트 진행바 색 변경
        private IEnumerator ChangeBarColor(Image barImage)
        {
            float time = 0f;
            float targetTime = ValueData.QuestBarColorChangeTime;

            ColorUtility.TryParseHtmlString(ValueData.QuestBarImageColorString, out Color targetColor);

            yield return new WaitForSeconds(ValueData.QuestBarShowTime * 0.5f);

            barImage.color = Color.white;
            while (time < 1f)
            {
                time += Time.deltaTime / targetTime;
                barImage.color = Color.Lerp(Color.white, targetColor, time);
                yield return null;
            }
        }


        // 개수
        // 바 이미지 크기 변경
        private void ActivateBarImages(int imageCount)
        {
            activatedBarImages.Clear();

            // 전체 크기를 알아야함
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


        // 큐에서 비활성화 하기
        private void DeactivateBarImages()
        {

        }

    }
}

