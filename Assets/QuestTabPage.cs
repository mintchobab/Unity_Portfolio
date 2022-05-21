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


    // 퀘스트 버튼 생성
    // 해당 퀘스트 정보를 받으면 버튼을 만듬
    private void MakeQuestButton()
    {

    }
}
