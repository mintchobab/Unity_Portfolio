using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace lsy
{
    public class UIManager : IManager
    {
        private readonly string RootCanvasName = "CanvasRoot";

        private GameObject rootCanvas;

        // �˾��� ������ �� �־����......
        public DialogueUIController DialogueController { get; private set; }
        public InputUIController InputController { get; private set; }
        public QuestUIController QuestUIController { get; private set; }
        public InventoryUIController InventoryUIController { get; private set; }


        public void Initialize()
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


        // ������ Canvas ����
        private void MakeCanvas()
        {
            InputController = Managers.Instance.ResourceManager.Instantiate<InputUIController>(ResourcePath.InputCanvas, rootCanvas.transform);
            DialogueController = Managers.Instance.ResourceManager.Instantiate<DialogueUIController>(ResourcePath.DialogueCanvas, rootCanvas.transform);
            QuestUIController = Managers.Instance.ResourceManager.Instantiate<QuestUIController>(ResourcePath.QuestCanvas, rootCanvas.transform);
            InventoryUIController = Managers.Instance.ResourceManager.Instantiate<InventoryUIController>(ResourcePath.InventoryCanvas, rootCanvas.transform);
        }
    }
}
