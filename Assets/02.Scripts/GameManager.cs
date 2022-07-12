using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class GameManager : Singleton<GameManager>
    {
        public override void Init()
        {

        }

        private void Start()
        {
            Managers.Instance.QuestManager.SetQuestToNPC(2000);
            Managers.Instance.EquipInventoryManager.AddEquipItem(0);
            Managers.Instance.EquipInventoryManager.AddEquipItem(10);
        }

    }
}
