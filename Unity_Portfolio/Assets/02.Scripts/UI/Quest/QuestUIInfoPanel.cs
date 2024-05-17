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


        private readonly Vector2 openPosition = new Vector2(-600f, 0f);
        private readonly Vector2 closePosition = new Vector2(0f, 0f);
        private readonly float panelMoveTime = 0.35f;


        public void Initailize()
        {
            rect = GetComponent<RectTransform>();

            showButton.onClick.AddListener(OnClickShowButton);

            contentsPanel.gameObject.SetActive(false);

            InitailizeTexts();
        }

        private void OnEnable()
        {
            questManager.onQuestChanged += OnQuestChanged;
            questManager.onChangedCurrentQuestCount += OnCurrentQuestItemCountChanged;
        }

        private void OnDisable()
        {
            if (questManager != null)
            {
                questManager.onQuestChanged -= OnQuestChanged;
                questManager.onChangedCurrentQuestCount -= OnCurrentQuestItemCountChanged;
            }            
        }


        private void InitailizeTexts()
        {
            outsideQuestName.text = string.Empty;
            outsideQuestGoal.text = string.Empty;

            questGoalText.text = string.Empty;
            questContentText.text = string.Empty;
            questRewardText.text = string.Empty;
        }


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
                QuestTable.TableData tableData = questManager.CurrentQuest.GetQuestTableData();

                outsideQuestName.text = StringManager.Get(tableData.Name);
                questNameText.text = StringManager.Get(tableData.Name);
                questContentText.text = StringManager.Get(tableData.Content);

                // Goal
                string strGoal = $"{StringManager.Get(tableData.Goal)} {questManager.CurrentQuest.CurrentCount}/{questManager.CurrentQuest.GoalCount}";
                outsideQuestGoal.text = strGoal;
                questGoalText.text = strGoal;

                // Reward
                string resultStr = $"{tableData.Exp} exp\n{tableData.Gold} gold";

                for (int i = 0; i < questManager.CurrentQuest.RewardList.Count; i++)
                {
                    int itemId = questManager.CurrentQuest.RewardList[i].Item1;

                    if (Tables.EquipmentItemTable.IsExist(itemId))
                    {
                        resultStr += $"\n{StringManager.Get(Tables.EquipmentItemTable[itemId].Name)}";
                    }
                    else if (Tables.ItemTable.IsExist(itemId))
                    {
                        resultStr += $"\n{StringManager.Get(Tables.ItemTable[itemId].Name)} : {questManager.CurrentQuest.RewardList[i].Item2}";
                    }
                }

                questRewardText.text = resultStr;
            }
        }

        private void OnCurrentQuestItemCountChanged()
        {
            string str = StringManager.Get(questManager.CurrentQuest.GetQuestTableData().Goal) +
                $" ({questManager.CurrentQuest.CurrentCount}/{questManager.CurrentQuest.GoalCount})";

            outsideQuestGoal.text = str;
            questGoalText.text = str;
        }


        private void OnClickShowButton()
        {
            if (isPanelMoving)
                return;

            if (!isOpen)
                StartCoroutine(OpenPanel());
            else
                StartCoroutine(ClosePanel());
        }


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
