using System;
using System.Collections.Generic;
using UnityEngine;
using lsy;

public class QuestTable : ScriptableObject
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
		public int NpcID;
		public string Name;
		public string Goal;
		public string Content;
		public int NextQuestID;
		public string StartDialogues;
		public string ProgressingDialogues;
		public string EndDialogues;
		public string Collect;
		public string Kill;
		public int Exp;
		public int Gold;
		public string Reward;
	}

	public void AddData(TableData data)
	{
		datas.Add(data);
	}
}

