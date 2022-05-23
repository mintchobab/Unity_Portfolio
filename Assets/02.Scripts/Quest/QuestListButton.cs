using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListButton : MonoBehaviour
{
    [SerializeField]
    private Text questNameText;

    private Button button;
    public Quest CurrentQuest { get; private set; }


    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // Localize 해야함
    public void SetQuest(Quest quest)
    {
        this.CurrentQuest = quest;
        questNameText.text = StringManager.GetLocalizedQuestName(quest.questName);
    }


    // 현재 퀘스트에 대한 내용을 window에 띄워야함
    // 내 퀘스트를 UIController에 넘겨주자
    public void AddButtonEvent(Action<Quest> action)
    {
        if (!button)
            button = GetComponent<Button>();

        button.onClick.AddListener(() => action?.Invoke(CurrentQuest));
    }


    

}
