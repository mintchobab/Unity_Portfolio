using UnityEngine;

public static class Tables
{
	public static EquipmentItemTable EquipmentItemTable;
	public static ItemTable ItemTable;
	public static LanguageTable LanguageTable;
	public static NPCTable NPCTable;
	public static QuestTable QuestTable;
	public static SkillTable SkillTable;

	static Tables()
	{
		if (EquipmentItemTable == null)
			EquipmentItemTable = Load<EquipmentItemTable>();

		if (ItemTable == null)
			ItemTable = Load<ItemTable>();

		if (LanguageTable == null)
			LanguageTable = Load<LanguageTable>();

		if (NPCTable == null)
			NPCTable = Load<NPCTable>();

		if (QuestTable == null)
			QuestTable = Load<QuestTable>();

		if (SkillTable == null)
			SkillTable = Load<SkillTable>();

	}

	public static T Load<T>() where T : ScriptableObject
	{
		T[] asset = Resources.LoadAll<T>("");

		if (asset == null || asset.Length != 1)
			throw new System.Exception($"{nameof(Tables)} : Tables Load Error");

		return asset[0];
	}
}
