using System;
using System.Collections.Generic;
using UnityEngine;
using lsy;

public class EquipmentItemTable : ScriptableObject
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
		public string Explanation;
		public string Parts;
		public float Hp;
		public float OffensivePower;
		public float DefensivePower;
	}

	public void AddData(TableData data)
	{
		datas.Add(data);
	}
}

