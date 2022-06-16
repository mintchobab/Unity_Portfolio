using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace lsy
{
    public class UIManager : IManager
    {
        private readonly string RootCanvasName = "CanvasRoot";

        private GameObject rootCanvas;

        // 팝업도 생성할 수 있어야함......
        public InputUIController InputUIController { get; private set; }
        public SystemUIController SystemUIController { get; private set; }
        public DialogueUIController DialogueUIController { get; private set; }
        public QuestUIController QuestUIController { get; private set; }
        public InventoryUIController InventoryUIController { get; private set; }
        public EquipUIController EquipUIController { get; private set; }
        //public WorldUIController WorldUIController { get; private set; }


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


        // 프리팹 Canvas 생성
        private void MakeCanvas()
        {
            InputUIController = Managers.Instance.ResourceManager.Instantiate<InputUIController>(ResourcePath.InputCanvas, rootCanvas.transform);
            SystemUIController = Managers.Instance.ResourceManager.Instantiate<SystemUIController>(ResourcePath.SystemCanvas, rootCanvas.transform);
            DialogueUIController = Managers.Instance.ResourceManager.Instantiate<DialogueUIController>(ResourcePath.DialogueCanvas, rootCanvas.transform);
            QuestUIController = Managers.Instance.ResourceManager.Instantiate<QuestUIController>(ResourcePath.QuestCanvas, rootCanvas.transform);
            InventoryUIController = Managers.Instance.ResourceManager.Instantiate<InventoryUIController>(ResourcePath.InventoryCanvas, rootCanvas.transform);
            EquipUIController = Managers.Instance.ResourceManager.Instantiate<EquipUIController>(ResourcePath.EquipInventoryCanvas, rootCanvas.transform);
            //WorldUIController = Managers.Instance.ResourceManager.Instantiate<WorldUIController>(ResourcePath.WorldCanvas, rootCanvas.transform);
        }

    }}
