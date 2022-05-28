using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy 
{
    public class QuestUIController : SceneUI
    {
        [SerializeField]
        private QuestTabPage progressQuestPage;

        [SerializeField]
        private QuestTabPage completedQuestPage;

        [SerializeField]
        private QuestDetailWindow questDetailWindow;

        private QuestManager questManager => Managers.Instance.QuestManager;


        private int progressingCount = 0;
        private int completedCount = 0;


        protected override void Awake()
        {
            base.Awake();
            Hide();
            Initialize();
        }


        // 1. ����Ʈ ����Ʈ�� ī��Ʈ�� ����Ǹ�
        // 2. �߰� ���� �������� �����ϰ�
        // 3. �װſ� �°� TabPage�� ��ư �߰� / ����
        private void Initialize()
        {
            // ó������ ����Ʈ�� �޾��� �ִ� ���� (Save & Load�� ����)
            progressQuestPage.AddQuestButton(questManager.ProgressingList, ShowQuestDetail);
            completedQuestPage.AddQuestButton(questManager.CompletedList, ShowQuestDetail);

            progressingCount = questManager.ProgressingList.Count;
            completedCount = questManager.CompletedList.Count;

            questManager.onChangeProgressingList += OnChangeProgressingList;

            questDetailWindow.gameObject.SetActive(false);
        }


        public void ShowQuestDetail(Quest quest)
        {
            questDetailWindow.SetQuest(quest);
            questDetailWindow.gameObject.SetActive(true);
        }


        private void OnChangeProgressingList(Quest quest, int count)
        {
            if (progressingCount < count)
            {
                progressQuestPage.AddQuestButton(quest, ShowQuestDetail);
                progressingCount = count;
            }
            else
            {
                progressQuestPage.RemoveQuestButton(quest);
                progressingCount = count;
            }
        }


        private void OnChangeCompletedList(Quest quest, int count)
        {
            //if ( > count)
            //    progressQuestPage.AddQuestButton(quest);
            //else
            //    progressQuestPage.RemoveQuestButton(quest);
        }
    }
}
