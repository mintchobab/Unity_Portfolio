using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class QuestUIInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private RectTransform contentsPanel;

        [SerializeField]
        private Button showButton;

        [SerializeField]
        private Text outsideQuestName;

        [SerializeField]
        private Text outsideQuestGoal;

        [SerializeField]
        private Text questNameText;

        [SerializeField]
        private Text questGoalText;

        [SerializeField]
        private Text questContentText;

        [SerializeField]
        private Text questRewardText;

        private RectTransform rect;


        private QuestManager questManager => Managers.Instance.QuestManager;
        private ItemManager itemManager => Managers.Instance.ItemManager;


        private bool isOpen;
        private bool isPanelMoving;


        private readonly Vector2 openPosition = new Vector2(1300f, 0f);
        private readonly Vector2 closePosition = new Vector2(1920f, 0f);
        private readonly float panelMoveTime = 0.35f;


        public void Initailize()
        {
            rect = GetComponent<RectTransform>();

            questManager.onQuestChanged += OnQuestChanged;
            questManager.onChangedCurrentQuestCount += OnCurrentQuestItemCountChanged;

            showButton.onClick.AddListener(OnClickShowButton);

            contentsPanel.gameObject.SetActive(false);

            InitailizeTexts();
        }


        private void InitailizeTexts()
        {
            outsideQuestName.text = string.Empty;
            outsideQuestGoal.text = string.Empty;

            questGoalText.text = string.Empty;
            questContentText.text = string.Empty;
            questRewardText.text = string.Empty;
        }


        // 퀘스트를 받거나 퀘스트가 끝났을 때 UI 변경
        private void OnQuestChanged()
        {
            // 퀘스트가 없을 때
            if (questManager.CurrentQuest == null)
            {
                outsideQuestName.text = string.Empty;
                outsideQuestGoal.text = string.Empty;

                questNameText.text = string.Empty;
                questGoalText.text = string.Empty;
                questContentText.text = string.Empty;
                questRewardText.text = string.Empty;
            }
            // 퀘스트가 있을 때
            else
            {
                Quest quest = questManager.CurrentQuest.Quest;

                outsideQuestName.text = StringManager.GetLocalizedQuestName(quest.questName);
                questNameText.text = StringManager.GetLocalizedQuestName(quest.questName);
                questContentText.text = StringManager.GetLocalizedQuestContent(quest.content);
                
                SetQuestGoalText(quest);
                SetQuestRewardText(quest);
            }
        }


        // 퀘스트 목표 텍스트 변경
        private void SetQuestGoalText(Quest quest)
        {
            string str = StringManager.GetLocalizedQuestGoal(quest.goal) +
                    $" ({questManager.CurrentQuest.CurrentCount}/{questManager.CurrentQuest.GoalCount})";

            outsideQuestGoal.text = str;
            questGoalText.text = str;
        }


        // 퀘스트 보상 텍스트 변경
        private void SetQuestRewardText(Quest quest)
        {
            string resultStr = $"{quest.reward.exp} exp\n{quest.reward.gold} gold";

            for (int i = 0; i < quest.reward.items.Count; i++)
            {
                RewardItem reward = quest.reward.items[i];

                // 장비
                if (reward.itemCount < 0)
                {
                    EquipItem item = itemManager.GetEquipItem(reward.itemId);
                    string name = StringManager.GetLocalizedItemName(item.name);

                    resultStr += $"\n{name}";
                }
                // 소모품
                else
                {
                    CountableItem item = itemManager.GetCountableItem(reward.itemId);
                    string name = StringManager.GetLocalizedItemName(item.name);

                    resultStr += $"\n{name} : {reward.itemCount}";
                }
            }

            questRewardText.text = resultStr;
        }



        // 퀘스트 아이템의 개수 변경
        private void OnCurrentQuestItemCountChanged()
        {
            string str = StringManager.GetLocalizedQuestGoal(questManager.CurrentQuest.Quest.goal) +
                $" ({questManager.CurrentQuest.CurrentCount}/{questManager.CurrentQuest.GoalCount})";

            outsideQuestGoal.text = str;
            questGoalText.text = str;
        }



        // 퀘스트창 버튼 눌렀을 때
        private void OnClickShowButton()
        {
            if (isPanelMoving)
                return;

            if (!isOpen)
                StartCoroutine(OpenPanel());
            else
                StartCoroutine(ClosePanel());
        }


        // 퀘스트창 열림
        private IEnumerator OpenPanel()
        {
            isPanelMoving = true;
            contentsPanel.gameObject.SetActive(true);

            outsideQuestName.gameObject.SetActive(false);
            outsideQuestGoal.gameObject.SetActive(false);

            float time = 0f;
            Vector2 currentPos = rect.anchoredPosition;

            while (time < 1)
            {
                time += Time.deltaTime / panelMoveTime;
                rect.anchoredPosition = Vector3.Lerp(currentPos, openPosition, time);
                yield return null;
            }

            isOpen = true;
            isPanelMoving = false;

            showButton.transform.localRotation = Quaternion.Euler(0, 0, 180f);
        }


        // 퀘스트창 닫힘
        private IEnumerator ClosePanel()
        {
            isPanelMoving = true;

            float time = 0f;
            Vector2 currentPos = rect.anchoredPosition;

            while (time < 1)
            {
                time += Time.deltaTime / panelMoveTime;
                rect.anchoredPosition = Vector3.Lerp(currentPos, closePosition, time);
                yield return null;
            }

            isOpen = false;
            isPanelMoving = false;
            contentsPanel.gameObject.SetActive(false);

            outsideQuestName.gameObject.SetActive(true);
            outsideQuestGoal.gameObject.SetActive(true);

            showButton.transform.localRotation = Quaternion.identity;
        }

    }
}
