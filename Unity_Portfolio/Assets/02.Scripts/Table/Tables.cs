using UnityEngine;

public static class Tables
{
	public static ConsumableItemTable ConsumableItemTable;
	public static EquipmentItemTable EquipmentItemTable;
	public static LanguageTable LanguageTable;
	public static NPCTable NPCTable;
	public static SkillTable SkillTable;

	static Tables()
	{
		if (ConsumableItemTable == null)
			ConsumableItemTable = Load<ConsumableItemTable>();

		if (EquipmentItemTable == null)
			EquipmentItemTable = Load<EquipmentItemTable>();

		if (LanguageTable == null)
			LanguageTable = Load<LanguageTable>();

		if (NPCTable == null)
			NPCTable = Load<NPCTable>();

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
