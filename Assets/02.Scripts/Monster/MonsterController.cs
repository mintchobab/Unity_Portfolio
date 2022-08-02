using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class MonsterController : MonoBehaviour
    {
        private Animator anim;
        private HpController hpController;

        private int hashTakeDamage = Animator.StringToHash("takeDamage");

        private void Awake()
        {
            anim = GetComponent<Animator>();
            hpController = GetComponent<HpController>();
        }


        private void OnEnable()
        {
            hpController.onTakeDamage += OnTakenDamage;
        }


        private void OnDisable()
        {
            hpController.onTakeDamage -= OnTakenDamage;
        }


        private void OnTakenDamage()
        {
            Debug.LogWarning("½ÇÇà");
            anim.SetTrigger(hashTakeDamage);
        }

        public void DestorySelf()
        {
            Destroy(gameObject);
        }
    }
}
