using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class GameManager : Singleton<GameManager>
    {
        private bool isCombating = false;

        [SerializeField]
        private GameObject[] testWolfs;


        public override void Init()
        {
        }


        private void Start()
        {
            Managers.Instance.SoundManager.Play("BGM04town3", SoundType.BGM);


            //Managers.Instance.QuestManager.SetQuestToNPC(2000);
            Managers.Instance.QuestManager.SetQuestToNPC(2002);

            Managers.Instance.InventoryManager.AddCountableItem(204, 5);

            Managers.Instance.EquipInventoryManager.AddEquipItem(0);
            Managers.Instance.EquipInventoryManager.AddEquipItem(1);
            Managers.Instance.EquipInventoryManager.AddEquipItem(10);
            Managers.Instance.EquipInventoryManager.AddEquipItem(11);
            Managers.Instance.EquipInventoryManager.AddEquipItem(20);
            Managers.Instance.EquipInventoryManager.AddEquipItem(21);
            Managers.Instance.EquipInventoryManager.AddEquipItem(30);
            Managers.Instance.EquipInventoryManager.AddEquipItem(31);
            Managers.Instance.EquipInventoryManager.AddEquipItem(40);
            Managers.Instance.EquipInventoryManager.AddEquipItem(41);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < testWolfs.Length; i++)
                {
                    testWolfs[i].gameObject.SetActive(true);
                }


                //if (!isCombating)
                //{
                //    isCombating = true;
                //    Managers.Instance.CombatManager.StartCombat();
                //}
                //else
                //{
                //    isCombating = false;
                //    Managers.Instance.CombatManager.EndCombat();
                //}               
            }
        }


        // 플레이어가 해당 지점에 들어갔을 때
        public void TriggerTest()
        {
            // 퀘스트를 받은 상태에서만 하기
            if (Managers.Instance.QuestManager.CurrentQuest == null)
                return;

            if (Managers.Instance.QuestManager.CurrentQuest.Quest.questId == 2002)
            {
                StartCoroutine(WolfCutScene());
            }            
        }


        private IEnumerator WolfCutScene()
        {
            Managers.Instance.UIManager.MainUIController.gameObject.SetActive(false);
            Managers.Instance.UIManager.QuestUIController.gameObject.SetActive(false);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.clear, Color.black, 3f);
            yield return new WaitForSeconds(3f);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.black, Color.clear, 3f);
            yield return new WaitForSeconds(5f);

            Managers.Instance.UIManager.MainUIController.gameObject.SetActive(true);
            Managers.Instance.UIManager.QuestUIController.gameObject.SetActive(true);
        }
    }
}
