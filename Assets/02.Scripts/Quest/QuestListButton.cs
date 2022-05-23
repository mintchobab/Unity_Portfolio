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

    // Localize �ؾ���
    public void SetQuest(Quest quest)
    {
        this.CurrentQuest = quest;
        questNameText.text = StringManager.GetLocalizedQuestName(quest.questName);
    }


    // ���� ����Ʈ�� ���� ������ window�� �������
    // �� ����Ʈ�� UIController�� �Ѱ�����
    public void AddButtonEvent(Action<Quest> action)
    {
        if (!button)
            button = GetComponent<Button>();

        button.onClick.AddListener(() => action?.Invoke(CurrentQuest));
    }


    

}
