using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class MinimapCamera : MonoBehaviour
    {
        [SerializeField]
        private GameObject minimapObject;

        private Transform playerTr;

        private float originHeight;


        private void Awake()
        {
            playerTr = FindObjectOfType<PlayerController>().transform;

            originHeight = transform.position.y;
            minimapObject.SetActive(true);
        }


        private void LateUpdate()
        {
            transform.position = new Vector3(playerTr.position.x, originHeight, playerTr.position.z);
        }
    }
}
