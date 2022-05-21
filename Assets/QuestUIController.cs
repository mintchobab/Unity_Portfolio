using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIController : SceneUI
{
    private List<QuestTabButton> tabButtons;




    [SerializeField]
    private QuestTabPage progressQuestPage;

    [SerializeField]
    private QuestTabPage completedQuestPage;


    private Transform questButtonParent;

    private Sprite defaultButtonImage;
    private Sprite selectedButtonImage;


    private QuestManager questManager => Managers.Instance.QuestManager;


    private List<Quest> progressingQuestList;
    private List<Quest> completedQuestList;






    protected override void Awake()
    {
        base.Awake();
        Hide();
        Init();
    }

    private void Init()
    {
        questButtonParent = GetComponentInChildren<VerticalLayoutGroup>().transform;

        progressQuestPage.AddQuestButton(questManager.GetProgressingQuestList());
        completedQuestPage.AddQuestButton(questManager.GetCompletedQuestList());

        //progressTabButton.onClick.AddListener(OnClickProgressTab);
        //completeTabButton.onClick.AddListener(OnClickCompleteTab);
    }








    private void OnClickProgressTab()
    {

    }

    private void OnClickCompleteTab()
    {

    }

    // 진행중 퀘스트 버튼 클릭시

    // 완료된 퀘스트 버튼 클릭시
}
