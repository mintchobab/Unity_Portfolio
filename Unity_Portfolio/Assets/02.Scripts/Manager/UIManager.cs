using UnityEngine;

namespace lsy
{
    public class UIManager : IManager
    {
        private readonly string RootCanvasName = "CanvasRoot";

        private GameObject rootCanvas;

        public MainUIController MainUIController { get; private set; }
        public SystemUIController SystemUIController { get; private set; }
        public DialogueUIController DialogueUIController { get; private set; }
        public QuestUIController QuestUIController { get; private set; }
        public InventoryUIController InventoryUIController { get; private set; }
        public EquipUIController EquipUIController { get; private set; }


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


        private void MakeCanvas()
        {
            MainUIController = Managers.Instance.ResourceManager.Instantiate<MainUIController>(ResourcePath.MainCanvas, rootCanvas.transform);
            SystemUIController = Managers.Instance.ResourceManager.Instantiate<SystemUIController>(ResourcePath.SystemCanvas, rootCanvas.transform);
            DialogueUIController = Managers.Instance.ResourceManager.Instantiate<DialogueUIController>(ResourcePath.DialogueCanvas, rootCanvas.transform);
            QuestUIController = Managers.Instance.ResourceManager.Instantiate<QuestUIController>(ResourcePath.QuestCanvas, rootCanvas.transform);
            InventoryUIController = Managers.Instance.ResourceManager.Instantiate<InventoryUIController>(ResourcePath.InventoryCanvas, rootCanvas.transform);
            EquipUIController = Managers.Instance.ResourceManager.Instantiate<EquipUIController>(ResourcePath.EquipInventoryCanvas, rootCanvas.transform);
        }
    }
}
