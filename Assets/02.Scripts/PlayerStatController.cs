using System;
using UnityEngine;



namespace lsy
{
    public class PlayerStatController : MonoBehaviour
    {
        [field: SerializeField]
        public PlayerStat PlayerStat { get; private set; }

        public event Action onChangedStat;


        private EquipInventoryManager equipInventoryManager => Managers.Instance.EquipInventoryManager;


        private void Start()
        {
            equipInventoryManager.onEquipedItem += OnEquipedItem;
            equipInventoryManager.onUnEquipedItem += OnUnEquipedItem;
        }



        // ¿Â∫Ò ¿Â¬¯ µ∆¿ª ∂ß
        private void OnEquipedItem(EquipType type, EquipItem item)
        {
            Stat stat = PlayerStat.playerEquipStat[type];

            stat.hp = item.hp;
            stat.offensivePower = item.offensivePower;
            stat.defensivePower = item.defensivePower;

            onChangedStat?.Invoke();
        }


        // ¿Â∫Ò «ÿ¡¶ µ∆¿ª ∂ß
        private void OnUnEquipedItem(EquipType type, EquipItem item)
        {
            PlayerStat.ResetStat(type);

            onChangedStat?.Invoke();
        }

    }
}
