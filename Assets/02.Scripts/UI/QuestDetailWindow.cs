using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class QuestDetailWindow : MonoBehaviour
    {
        [SerializeField]
        private Text questName;

        [SerializeField]
        private Text questContents;

        [SerializeField]
        private Text questTask;

        [SerializeField]
        private Text questReward;


        public void SetQuest(Quest quest)
        {
            questName.text = StringManager.GetLocalizedQuestName(quest.questName);
            questContents.text = StringManager.GetLocalizedQuestContent(quest.content);
        }
    }
}
