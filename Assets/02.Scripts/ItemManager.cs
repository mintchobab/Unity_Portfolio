namespace lsy
{
    public class ItemManager : IManager
    {

        private JsonItem jsonItem => Managers.Instance.JsonManager.jsonItem;

        public void Initialize()
        {

        }


        // 로드해주는 역할
        public CountableItem GetConsumableItem(int itemId)
        {
            return jsonItem.consumables.Find(x => x.id == itemId);
        }

        public EquipItem GetEquipItem(int itemId)
        {
            return jsonItem.equips.Find(x => x.id == itemId);
        }

        public CountableItem GetEtcItem(int itemId)
        {
            return jsonItem.etcs.Find(x => x.id == itemId);
        }
    }
}


