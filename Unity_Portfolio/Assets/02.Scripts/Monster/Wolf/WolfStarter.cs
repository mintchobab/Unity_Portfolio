using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class WolfStarter : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.EnterBattleTrigger(gameObject);
            }
        }
    }
}
