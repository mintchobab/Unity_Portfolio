using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : IManager
{
    private readonly string RootCanvasName = "CanvasRoot";

    private GameObject rootCanvas;

    // 팝업도 생성할 수 있어야함......

    public DialogueUIController DialogueController { get; private set; }
    public InputUIController InputController { get; private set; }
    public QuestUIController QuestUIController { get; private set; }


    public void Init()
    {
        MakeRoot();
        MakeCanvas();
    }


    private void MakeRoot()
    {
        rootCanvas = GameObject.Find(RootCanvasName);

        if (rootCanvas == null)
        {
            rootCanvas = new GameObject(RootCanvasName);
        }
    }


    // 프리팹 Canvas 생성
    private void MakeCanvas()
    {
        InputController = Managers.Instance.ResourceManager.Instantiate<InputUIController>(ResourceFolderPath.InputCanvas, rootCanvas.transform);
        Managers.Instance.ResourceManager.Instantiate<Canvas>(ResourceFolderPath.InventoryCanvas, rootCanvas.transform);

        DialogueController = Managers.Instance.ResourceManager.Instantiate<DialogueUIController>(ResourceFolderPath.DialogueCanvas, rootCanvas.transform);
        QuestUIController = Managers.Instance.ResourceManager.Instantiate<QuestUIController>(ResourceFolderPath.QuestCanvas, rootCanvas.transform);
    }    

}
