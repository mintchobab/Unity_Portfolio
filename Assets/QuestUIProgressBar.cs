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



        // �ʱ�ȭ �κ��� �ٽ� �����غ���....................................................................
        public void Initialize()
        {
            rectTransform = GetComponent<RectTransform>();

            questManager.onQuestChanged += OnQuestChanged;
            questManager.onCurrentQuestItemCountChanged += OnCurrentQuestItemCountChanged;
        }


        // ����Ʈ�� ����Ǹ�
        // �� ����Ʈ�� ��ǥ ������ �°� �̹����� ��ġ��
        private void OnQuestChanged()
        {
            if (questManager.CurrentQuest == null)
                return;

            prevCount = 0;

            Quest quest = questManager.CurrentQuest.Quest;

            // ���� ����Ʈ�� ��
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

            // �������� ���� �Ǵ� ���
            if (currentCount < prevCount)
            {
                for (int i = barImages.Length - 1; i >= currentCount; i--)
                {
                    if (!barImages[i].gameObject.activeSelf)
                        continue;

                    barImages[i].gameObject.SetActive(false);
                }
            }
            // �������� �߰��Ǵ� ���
            else
            {
                barImages[currentCount - 1].gameObject.SetActive(true);
                StartCoroutine(ChangePosition());
                StartCoroutine(ChangeBarColor(barImages[currentCount - 1]));
            }

            prevCount = currentCount;
        }


        // ����Ʈ ����� ��ġ ����
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


        // ����Ʈ ����� �� ����
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


        // ����
        // �� �̹��� ũ�� ����
        private void ActivateBarImages(int imageCount)
        {
            activatedBarImages.Clear();

            // ��ü ũ�⸦ �˾ƾ���
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


        // ť���� ��Ȱ��ȭ �ϱ�
        private void DeactivateBarImages()
        {

        }

    }
}

