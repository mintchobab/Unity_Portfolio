namespace lsy
{
    public class ItemManager : IManager
    {

        private JsonItem jsonItem => Managers.Instance.JsonManager.jsonItem;

        public void Init()
        {

        }

        public EquipItem GetEquipItem(int itemId)
        {
            return jsonItem.equips.Find(x => x.id == itemId);
        }

        public CountableItem GetCountableItem(int itemId)
        {
            return jsonItem.countables.Find(x => x.id == itemId);
        }
    }
}
