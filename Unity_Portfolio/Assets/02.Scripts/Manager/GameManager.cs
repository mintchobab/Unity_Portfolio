using System.Collections;
using UnityEngine;

namespace lsy
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private GameObject[] wolves;

        [SerializeField]
        private GameObject bossWolf;

        private int wolfDieCount = 0;
        private readonly float fadeTime = 2f;


        public override void Init()
        {
        }


        private void Start()
        {
            Managers.Instance.SoundManager.Play("BGM04town3", SoundType.BGM);

            Managers.Instance.QuestManager.SetQuestToNPC(2000);

            // TODO
            Managers.Instance.InventoryManager.AddItem(10000, 99);

            //TODO
            Managers.Instance.InventoryManager.AddItem(30031);
            Managers.Instance.InventoryManager.AddItem(30021);
            Managers.Instance.InventoryManager.AddItem(30001);
            Managers.Instance.InventoryManager.AddItem(30041);
            Managers.Instance.InventoryManager.AddItem(30010);
            Managers.Instance.InventoryManager.AddItem(30011);
            Managers.Instance.InventoryManager.AddItem(30040);
            Managers.Instance.InventoryManager.AddItem(30030);
            Managers.Instance.InventoryManager.AddItem(30020);
            Managers.Instance.InventoryManager.AddItem(30000);
        }


        public void DieWolf()
        {
            wolfDieCount++;

            if (wolfDieCount == 3)
            {
                StartCoroutine(BossWolfCutScene());
            }
            else if (wolfDieCount == 4)
            {
                StartCoroutine(EndGame());
            }
        }


        public void EnterBattleTrigger(GameObject triggerCollider)
        {
            if (Managers.Instance.QuestManager.CurrentQuest == null)
                return;

            triggerCollider.gameObject.SetActive(false);

            if (Managers.Instance.QuestManager.CurrentQuest.Quest.questId == 2002)
            {
                StartCoroutine(WolfCutScene());
            }            
        }


        private IEnumerator WolfCutScene()
        {
            Managers.Instance.UIManager.MainUIController.gameObject.SetActive(false);
            Managers.Instance.UIManager.QuestUIController.gameObject.SetActive(false);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.clear, Color.black, 2f);
            yield return new WaitForSeconds(2f);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.black, Color.clear, 2f);
            Managers.Instance.SoundManager.Play("BGM07battle1", SoundType.BGM);

            for (int i = 0; i < wolves.Length; i++)
            {
                wolves[i].gameObject.SetActive(true);
            }

            CameraController.Instance.MoveAndLooking(new Vector3(44f, 3.5f, 9.4f), wolves[1].transform);

            yield return new WaitForSeconds(7f);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.clear, Color.black, 2f);
            yield return new WaitForSeconds(fadeTime);
            
            CameraController.Instance.StartFolloing();
            Managers.Instance.CombatManager.StartCombat();

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.black, Color.clear, 2f);
            yield return new WaitForSeconds(fadeTime);

            Managers.Instance.UIManager.MainUIController.gameObject.SetActive(true);
            Managers.Instance.UIManager.QuestUIController.gameObject.SetActive(true);

            for (int i = 0; i < wolves.Length; i++)
            {
                wolves[i].GetComponent<WolfMovePath>().enabled = false;
                wolves[i].GetComponent<WolfBT>().enabled = true;
            }
        }


        private IEnumerator BossWolfCutScene()
        {
            Managers.Instance.UIManager.MainUIController.gameObject.SetActive(false);            

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.clear, Color.black, 2f);
            yield return new WaitForSeconds(fadeTime);

            Managers.Instance.UIManager.QuestUIController.gameObject.SetActive(false);
            bossWolf.gameObject.SetActive(true);

            CameraController.Instance.MoveAndLooking(new Vector3(56f, 3.5f, -1.15f), bossWolf.transform);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.black, Color.clear, 2f);
            yield return new WaitForSeconds(fadeTime);
            yield return new WaitForSeconds(2f);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.clear, Color.black, 2f);
            yield return new WaitForSeconds(fadeTime);

            CameraController.Instance.StartFolloing();

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.black, Color.clear, 2f);
            yield return new WaitForSeconds(fadeTime);

            Managers.Instance.UIManager.MainUIController.gameObject.SetActive(true);
            Managers.Instance.UIManager.QuestUIController.gameObject.SetActive(true);

            bossWolf.GetComponent<BossWolfController>().StartBT();

            yield return null;
        }


        private IEnumerator EndGame()
        {
            Managers.Instance.UIManager.MainUIController.gameObject.SetActive(false);
            Managers.Instance.UIManager.QuestUIController.gameObject.SetActive(false);

            Managers.Instance.UIManager.SystemUIController.FadeScreen(Color.clear, Color.black, 2f);
            yield return new WaitForSeconds(fadeTime);

            Managers.Instance.UIManager.SystemUIController.ShowEndText();
            Managers.Instance.SoundManager.Stop(SoundType.BGM);

            yield return new WaitForSeconds(10f);

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
