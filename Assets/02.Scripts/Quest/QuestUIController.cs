using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy 
{
    public class QuestUIController : SceneUI
    {
        [SerializeField]
        private RectTransform panel;

        [SerializeField]
        private Button closeButton;


        [SerializeField]
        private QuestTabPage progressQuestPage;

        [SerializeField]
        private QuestTabPage completedQuestPage;

        [SerializeField]
        private QuestDetailWindow questDetailWindow;

        private QuestManager questManager => Managers.Instance.QuestManager;

        private bool isPanelMoving;

        private int progressingCount = 0;
        private int completedCount = 0;




        private readonly Vector2 openPosition = new Vector2(1300f, 0f);
        private readonly Vector2 closePosition = new Vector2(1920f, 0f);
        private readonly float panelMoveTime = 0.35f;


        protected override void Awake()
        {
            base.Awake();
            Hide();
            Initialize();
        }


        public override void Show()
        {
            if (isPanelMoving)
                return;

            base.Show();
            panel.anchoredPosition = closePosition;
            StartCoroutine(OpenPanel());
        }

        private void OnClickCloseButton()
        {
            if (isPanelMoving)
                return;

            StartCoroutine(ClosePanel());
        }


        private IEnumerator OpenPanel()
        {
            isPanelMoving = true;

            float time = 0f;

            Vector2 currentPos = panel.anchoredPosition;

            while(time < 1)
            {
                time += Time.deltaTime / panelMoveTime;
                panel.anchoredPosition = Vector3.Lerp(currentPos, openPosition, time);
                yield return null;
            }

            isPanelMoving = false;
        }

        private IEnumerator ClosePanel()
        {
            isPanelMoving = true;

            float time = 0f;

            Vector2 currentPos = panel.anchoredPosition;

            while (time < 1)
            {
                time += Time.deltaTime / panelMoveTime;
                panel.anchoredPosition = Vector3.Lerp(currentPos, closePosition, time);
                yield return null;
            }

            isPanelMoving = false;
            Hide();
        }






        // 1. 퀘스트 리스트의 카운트가 변경되면
        // 2. 추가 인지 삭제인지 구분하고
        // 3. 그거에 맞게 TabPage의 버튼 추가 / 삭제
        private void Initialize()
        {
            closeButton.onClick.AddListener(OnClickCloseButton);

            // 처음부터 퀘스트가 받아져 있는 상태 (Save & Load를 위해)
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

    }
}
