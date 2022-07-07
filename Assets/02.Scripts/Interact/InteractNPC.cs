using UnityEngine;

namespace lsy
{
    [RequireComponent(typeof(WorldUIName))]
    public class InteractNpc : MonoBehaviour, IInteractable
    {
        // NPC Á¤º¸ 
        [field: SerializeField]
        public int NpcId { get; private set; }

        private GameObject exclamationMark;

        public Npc Npc { get; private set; }
        public Quest MyQuest { get; private set; }
        public string NpcName { get; private set; }


        protected void Awake()
        {
            SetNPCData();
            NpcName = StringManager.GetLocalizedNPCName(Npc.name);
        }



        public void Interact()
        {
            PlayerController.Instance.CheckInteract(this, transform);
        }


        public Sprite LoadButtonImage()
        {
            return Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconDialogue);
        }

        public Transform GetTransform()
        {
            return transform;
        }


        // Äù½ºÆ® ºÎ¿©ÇÏ±â
        public void SetQuest(Quest quest)
        {
            MyQuest = quest;

            // ´À³¦Ç¥ ¶ç¿ì±â
            exclamationMark = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.ExclamationMark, transform);
            exclamationMark.transform.localPosition = new Vector3(0f, 2.1f, 0f);
        }

        public void ResetQuest()
        {
            MyQuest = null;
        }



        private void SetNPCData()
        {
            JsonNpc dialogueData = Managers.Instance.JsonManager.jsonNPC;

            foreach (Npc npc in dialogueData.npcs)
            {
                if (npc.id.Equals(NpcId))
                    this.Npc = npc;
            }

            if (Npc == null)
                Debug.LogError("NPC Data Not Found");
        }

        public void DestoryExclamationMark()
        {
            if (exclamationMark)
            {
                Destroy(exclamationMark);
                exclamationMark = null;
            }
        }


    }
}
