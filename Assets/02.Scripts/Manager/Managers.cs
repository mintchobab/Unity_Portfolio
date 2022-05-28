using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class Managers : Singleton<Managers>
    {
        private JsonManager jsonManager = new JsonManager();
        private StringManager stringManager = new StringManager();
        private ResourceManager resourceManager = new ResourceManager();
        private UIManager uiManger = new UIManager();
        private QuestManager questManager = new QuestManager();
        private ItemManager itemManager = new ItemManager();
        private InventoryManager inventoryManager = new InventoryManager();

        public JsonManager JsonManager { get => jsonManager; }
        public StringManager StringManager { get => stringManager; }
        public ResourceManager ResourceManager { get => resourceManager; }
        public UIManager UIManager { get => uiManger; }
        public QuestManager QuestManager { get => questManager; }
        public ItemManager ItemManager { get => itemManager; }
        public InventoryManager InventoryManager { get => inventoryManager; }


        public override void Init()
        {
            DontDestroyOnLoad(this);

            JsonManager.Initialize();
            StringManager.Initialize();
            ResourceManager.Initialize();
            UIManager.Initialize();
            QuestManager.Initialize();
            ItemManager.Initialize();
            InventoryManager.Initialize();
        }
    }
}

