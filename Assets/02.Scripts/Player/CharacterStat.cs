using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterStat : MonoBehaviour
{
    [SerializeField]
    protected StatData statData;

    protected Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();


    protected virtual void Awake()
    {
        SetStat();
    }

    protected void SetStat()
    {
        stats.Add(StatType.Hp, new Stat("Hp", "Hp�Դϴ�.", statData.Hp));
        stats.Add(StatType.Mp, new Stat("Mp", "Mp�Դϴ�.", statData.Mp));
        stats.Add(StatType.Damage, new Stat("Damage", "Damage �Դϴ�.", statData.Damage));
        stats.Add(StatType.Armor, new Stat("Armor", "Armor �Դϴ�.", statData.Armor));
        stats.Add(StatType.MoveSpeed, new Stat("MoveSpeed", "Move Speed �Դϴ�.", statData.MoveSpeed));
    }


    public Stat FindStat(StatType type)
    {
        return stats[type];
    }

    #region Cutom Inspector

    [CustomEditor(typeof(CharacterStat))]
    public class PlayerStatEditor : Editor
    {
        private CharacterStat playerStat;

        private void OnEnable()
        {
            playerStat = target as CharacterStat;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Hp", playerStat.stats[StatType.Hp].GetValue().ToString());
                EditorGUILayout.LabelField("Damage", playerStat.stats[StatType.Damage].GetValue().ToString());
                EditorGUILayout.LabelField("Armor", playerStat.stats[StatType.Armor].GetValue().ToString());
                EditorGUILayout.LabelField("MoveSpeed", playerStat.stats[StatType.MoveSpeed].GetValue().ToString());
            }            
        }
    }

    #endregion

}
