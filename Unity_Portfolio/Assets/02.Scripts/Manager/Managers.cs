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
        //TODO
        //private EquipInventoryManager equipInventoryManager = new EquipInventoryManager();
        private PoolManager poolManager = new PoolManager();
        private CombatManager combatManager = new CombatManager();
        private SoundManager soundManager = new SoundManager();

        public JsonManager JsonManager { get => jsonManager; }
        public StringManager StringManager { get => stringManager; }
        public ResourceManager ResourceManager { get => resourceManager; }
        public UIManager UIManager { get => uiManger; }
        public QuestManager QuestManager { get => questManager; }
        public ItemManager ItemManager { get => itemManager; }
        public InventoryManager InventoryManager { get => inventoryManager; }
        // TODO
        //public EquipInventoryManager EquipInventoryManager { get => equipInventoryManager; }
        public PoolManager PoolManager { get => poolManager; }
        public CombatManager CombatManager { get => combatManager; }
        public SoundManager SoundManager { get => soundManager; }


        public override void Init()
        {
            DontDestroyOnLoad(this);

            JsonManager.Init();
            StringManager.Init();
            ResourceManager.Init();
            UIManager.Init();
            SoundManager.Init();
            QuestManager.Init();
            ItemManager.Init();
            InventoryManager.Init();
            // TODO
            //EquipInventoryManager.Init();
            PoolManager.Init();
            CombatManager.Init();            
        }
    }
}
