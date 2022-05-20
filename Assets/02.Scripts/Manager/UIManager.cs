using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : IManager
{
    private readonly string RootCanvasName = "CanvasRoot";
    private readonly string FolderPath = "UI/Canvas/";

    private GameObject rootCanvas;

    // 팝업도 생성할 수 있어야함......

    public DialogueUIController DialogueController { get; private set; }
    public InputUIController InputController { get; private set; }


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
        InputController = Managers.Instance.ResourceManager.Instantiate<InputUIController>($"{FolderPath}InputCanvas", rootCanvas.transform);
        Managers.Instance.ResourceManager.Instantiate<Canvas>($"{FolderPath}InventoryCanvas", rootCanvas.transform);
        DialogueController = Managers.Instance.ResourceManager.Instantiate<DialogueUIController>($"{FolderPath}DialogueCanvas", rootCanvas.transform);
    }    

}
