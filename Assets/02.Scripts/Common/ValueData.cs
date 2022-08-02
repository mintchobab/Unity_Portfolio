using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class ValueData
    {
        public static readonly string npcNameColorString = "#316DDB";
        public static readonly string collectionNameColorString = "#D0EC33";


        // Interact
        public static readonly float MiningTime = 3f;
        public static readonly float MiningDistance = 1.2f;

        public static readonly float SearchTime = 3f;
        public static readonly float SearchDistance = 1f;

        public static readonly float FishingTime = 3f;
        public static readonly float FishingDistance = 3.5f;

        public static readonly float NpcDistance = 1f;

        public static readonly float AfterInteractDelayTime = 3f;


        // Quest
        public static readonly string QuestBarImageColorString = "#FF8019";
        public static readonly float QuestBarColorChangeTime = 1f;
        public static readonly float QuestBarShowTime = 1f;
        public static readonly float QuestBarDelayTime = 2f;

        // UI
        public static readonly float SystemMessageFadeTime = 4f;

        public static readonly float SkillButtonTransitionTime = 0.28f;
        public static readonly float SkillButtonTransitionScale = 0.8f;

        // Monster
        // 이건 json으로 옮길지도??
        public static readonly float MonsterDestroyTime = 3f;

        public static readonly float WolfAttackBeforeTime = 1f;
        public static readonly float WolfAttackAfterTime = 1f;
    }
}



