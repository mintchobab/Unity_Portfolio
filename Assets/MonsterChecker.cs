using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class MonsterChecker : MonoBehaviour
    {
        private Collider coll;
        private InteractChecker interactChecker;


        private void Awake()
        {
            coll = GetComponent<Collider>();
            interactChecker = GetComponent<InteractChecker>();
        }


        private void OnEnable()
        {
            coll.enabled = false;
            coll.enabled = true;
        }
    }
}


