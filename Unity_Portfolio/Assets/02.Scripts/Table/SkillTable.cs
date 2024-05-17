using System;
using System.Collections.Generic;
using UnityEngine;
using lsy;

public class SkillTable : ScriptableObject
{
	[SerializeField]
	public List<TableData> datas = new List<TableData>();

	public TableData this[int index]
	{
		get
		{
			return datas.Find(x => x.ID == index);
		}
	}

	[Serializable]
	public class TableData
	{
		public int ID;
		public string Name;
		public string ResourceName;
		public float CoolTime;
		public int MinDamage;
		public int MaxDamage;
	}

	public void AddData(TableData data)
	{
		datas.Add(data);
	}
}

