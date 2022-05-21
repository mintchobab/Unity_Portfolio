using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListButton : MonoBehaviour
{
    [SerializeField]
    private Text questNameText;

    // Localize �ؾ���
    public void SetQuest(Quest quest)
    {
        questNameText.text = quest.questName;
    }
}
