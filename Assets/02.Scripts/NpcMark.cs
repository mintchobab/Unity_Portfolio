using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class NpcMark : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}


