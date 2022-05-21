using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTabPage : MonoBehaviour
{
    [field: SerializeField]
    public QuestTabType TabType { get; private set; }


    private List<QuestListButton> buttonList = new List<QuestListButton>();


    public void AddQuestButton(int questId)
    {

    }


    public void AddQuestButton(List<Quest> questList)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            QuestListButton questButton = Managers.Instance.ResourceManager.Instantiate<QuestListButton>(ResourceFolderPath.QuestListButton, transform);
            questButton.SetQuest(questList[i]);

            buttonList.Add(questButton);
        }
    }


    // ����Ʈ ��ư ����
    // �ش� ����Ʈ ������ ������ ��ư�� ����
    private void MakeQuestButton()
    {

    }
}
