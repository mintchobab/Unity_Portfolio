using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform testBody;

    private Camera cam;
    private NavMeshAgent navAgent;
    private Animator animator;
    private Joystick joystick;

    private bool isMoving = false;

    private int MOVE_HASH = Animator.StringToHash("isMove");

    private float rotSpeed = 10f;



    private bool canMoving = false;


    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        joystick = FindObjectOfType<Joystick>();
    }


    private void Start()
    {
        cam = Camera.main;
        navAgent.updateRotation = false;

        canMoving = true;

        joystick.onStickMove += OnStickMove;
        joystick.onEndStickMove += OnEndStickMove;
    }


    
    private void OnStickMove(Vector2 stickVector)
    {
        if (!isMoving)
        {
            isMoving = true;
            animator.SetBool(MOVE_HASH, true);
        }

        // 이동
        Vector3 direction = cam.transform.TransformDirection(stickVector);
        direction.y = 0f;
        direction.Normalize();
        transform.position += direction * 5f * Time.deltaTime;

        // 회전
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
    }

    private void OnEndStickMove()
    {
        if (isMoving)
        {
            isMoving = false;
            animator.SetBool(MOVE_HASH, false);
        }
    }
}

