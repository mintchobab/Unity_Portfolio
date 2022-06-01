using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace lsy
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float distance;

        private Transform player;
        private Quaternion originRotation;

        private InputUIController inputUIController => Managers.Instance.UIManager.InputController;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player").transform;
            originRotation = transform.rotation;
        }


        void LateUpdate()
        {
            Vector3 direction = originRotation * Vector3.forward;
            direction = Quaternion.Euler(inputUIController.TestRot) * direction;
            direction.Normalize();

            transform.position = (player.transform.position + new Vector3(0f, 0.8f, 0f)) - direction * distance;
            transform.rotation = Quaternion.LookRotation(direction);
        }


        private void Zoom()
        {
            distance -= Input.mouseScrollDelta.y;
            distance = Mathf.Clamp(distance, 7f, 12f);
        }
    }
}


