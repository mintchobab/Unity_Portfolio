public class PlayerStat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();

        //EquipmentSystem.Instance.onEquipItem += OnEquipItem;
        //EquipmentSystem.Instance.onUnEquipItem += OnUnEquipItem;
    }


    //private void OnEquipItem(EquipmentData data)
    //{
    //    stats[StatType.Hp].IncreaseEquipmentValue(data.Hp);
    //    stats[StatType.Damage].IncreaseEquipmentValue(data.Damage);
    //    stats[StatType.Armor].IncreaseEquipmentValue(data.Armor);
    //    stats[StatType.MoveSpeed].IncreaseEquipmentValue(data.MoveSpeed);
    //}


    //private void OnUnEquipItem(EquipmentData data)
    //{
    //    stats[StatType.Hp].DecreaseEquipmentValue(data.Hp);
    //    stats[StatType.Damage].DecreaseEquipmentValue(data.Damage);
    //    stats[StatType.Armor].DecreaseEquipmentValue(data.Armor);
    //    stats[StatType.MoveSpeed].DecreaseEquipmentValue(data.MoveSpeed);
    //}
}
