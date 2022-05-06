using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField]
    private float distance;

    private Transform player;
    private Quaternion originRotation;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        originRotation = transform.rotation;
    }

    void LateUpdate()
    {
        Vector3 direction = originRotation * Vector3.forward;
        direction.Normalize();

        transform.position = player.transform.position - direction * distance;
    }
}
