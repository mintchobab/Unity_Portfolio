using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class MonsterController : MonoBehaviour, IDamageable
    {
        public readonly int MaxHp = 100;
        public int currentHp = 100;

        public bool CheckIsDead()
        {
            return false;
        }

        public void TakeDamage(int damage)
        {
            currentHp -= damage;
            Debug.LogWarning("데미지를 받음");
        }
    }
}
