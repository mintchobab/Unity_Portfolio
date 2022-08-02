namespace lsy
{
    public class ItemManager : IManager
    {

        private JsonItem jsonItem => Managers.Instance.JsonManager.jsonItem;

        public void Init()
        {

        }


        // 로드해주는 역할
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


