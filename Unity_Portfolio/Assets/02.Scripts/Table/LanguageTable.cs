using System;
using System.Collections.Generic;
using UnityEngine;
using lsy;

public class LanguageTable : SingletonScriptableObject<LanguageTable>
{
	[SerializeField]
	public List<TableData> datas = new List<TableData>();

	public TableData this[string index]
	{
		get
		{
			return datas.Find(x => x.ID == index);
		}
	}

	[Serializable]
	public class TableData
	{
		public string ID;
		public string Korean;
		public string English;
	}

	public void AddData(TableData data)
	{
		datas.Add(data);
	}
}

