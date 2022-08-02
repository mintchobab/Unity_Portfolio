namespace lsy
{
    public static class ResourcePath
    {
        #region Load

        // UI/Cavnas
        public static readonly string MainCanvas = "Load/Prefab/UI/Canvas/MainCanvas";
        public static readonly string SystemCanvas = "Load/Prefab/UI/Canvas/SystemCanvas";
        public static readonly string DialogueCanvas = "Load/Prefab/UI/Canvas/DialogueCanvas";
        public static readonly string QuestCanvas = "Load/Prefab/UI/Canvas/QuestCanvas";
        public static readonly string InventoryCanvas = "Load/Prefab/UI/Canvas/InventoryCanvas";
        public static readonly string EquipInventoryCanvas = "Load/Prefab/UI/Canvas/EquipInventoryCanvas";

        // UI/Quest
        public static readonly string QuestListButton = "Load/Prefab/UI/Quest/QuestListButton";

        // WorldSpace UI
        public static readonly string WorldNameCanvas = "Load/Prefab/UI/World/WorldNameCanvas";
        public static readonly string WorldInteractCircleCanvas = "Load/Prefab/UI/World/WorldInteractCircleCanvas";
        public static readonly string WorldInteractGaugeCanvas = "Load/Prefab/UI/World/WorldInteractGaugeCanvas";
        public static readonly string WorldHpCanvas = "Load/Prefab/UI/World/WorldHpCanvas";

        // Image
        public static readonly string Empty = "Load/Texture/Empty";

        // Icon
        public static readonly string Icon = "Load/Texture/Icon";
        public static readonly string IconBasic = "Load/Texture/Icon/Icon_Basic";
        public static readonly string IconStop = "Load/Texture/Icon/Icon_Stop";
        public static readonly string IconDialogue = "Load/Texture/Icon/Icon_Dialogue";
        public static readonly string IconMining = "Load/Texture/Icon/Icon_Mining";
        public static readonly string IconSearch = "Load/Texture/Icon/Icon_Search";
        public static readonly string IconFishing = "Load/Texture/Icon/Icon_Fishing";
        public static readonly string IconCombat = "Load/Texture/Icon/Icon_Combat";

        // Item
        public static readonly string EquipItem = "Load/Texture/Item/Equip";
        public static readonly string ConsumableItem = "Load/Texture/Item/Consumable";
        public static readonly string MaterialItem = "Load/Texture/Item/Material";

        // Tool
        public static readonly string Pickax = "Load/Prefab/Tool/Pickax";
        public static readonly string FishingRod = "Load/Prefab/Tool/FishingRod";

        // Equip
        public static readonly string Equip = "Load/Prefab/Equip";

        // Pool
        public static readonly string DamageText = "Load/Prefab/Pool/DamageText";

        // Sound
        public static readonly string Sound = "Load/Sound";
        public static readonly string BGM = "Load/Sound/BGM";
        public static readonly string SFX = "Load/Sound/SFX";

        // ETC
        public static readonly string ExclamationMark = "Load/Prefab/Etc/ExclamationMark";
        public static readonly string QuestionMark = "Load/Prefab/Etc/QuestionMark";

        #endregion



        #region Non-Load

        public static readonly string InteractDialogue = "Image/Icon_Dialouge";
        public static readonly string InteractAxing = "Image/Icon_Axing";

        #endregion



        #region Json Data

        // Json
        public static readonly string NPCData = "Json/JsonNPCData";
        public static readonly string QuestData = "Json/JsonQuestData";
        public static readonly string StringData = "Json/JsonStringData";
        public static readonly string ItemData = "Json/JsonItemData";
        public static readonly string MonsterData = "Json/JsonMonsterData";
        public static readonly string SkillData = "Json/JsonSkillData";

        #endregion


        public static readonly string SlotRow = "UI/SlotRow";

        
        public static string GetItemSpritePathToType(ItemType itemType)
        {
            string path = string.Empty;

            switch (itemType)
            {
                case ItemType.Equipment:
                    path = EquipItem;
                    break;

                case ItemType.Consumable:
                    path = ConsumableItem;
                    break;

                case ItemType.Material:
                    path = MaterialItem;
                    break;
            }

            return path;
        }

        public static string GetItemSpritePathToTypeString(string itemType)
        {
            string path = string.Empty;

            switch (itemType) 
            {
                case nameof(ItemType.Equipment):
                    path = EquipItem;
                    break;

                case nameof(ItemType.Consumable):
                    path = ConsumableItem;
                    break;

                case nameof(ItemType.Material):
                    path = MaterialItem;
                    break;
            }

            return path;
        }


    }
}
