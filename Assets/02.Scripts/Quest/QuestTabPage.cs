using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class QuestTabPage : MonoBehaviour
    {
        [field: SerializeField]
        public QuestTabType TabType { get; private set; }


        private List<QuestListButton> buttonList = new List<QuestListButton>();



        public void AddQuestButton(Quest quest, Action<Quest> action = null)
        {
            QuestListButton questButton = Managers.Instance.ResourceManager.Instantiate<QuestListButton>(ResourcePath.QuestListButton, transform);
            questButton.SetQuest(quest);
            questButton.AddButtonEvent(action);

            buttonList.Add(questButton);
        }


        public void AddQuestButton(List<Quest> questList, Action<Quest> action = null)
        {
            for (int i = 0; i < questList.Count; i++)
            {
                QuestListButton questButton = Managers.Instance.ResourceManager.Instantiate<QuestListButton>(ResourcePath.QuestListButton, transform);
                questButton.SetQuest(questList[i]);
                questButton.AddButtonEvent(action);

                buttonList.Add(questButton);
            }
        }

        public void RemoveQuestButton(Quest quest)
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].CurrentQuest == quest)
                {
                    buttonList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
