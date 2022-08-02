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
    private Animator animator;
    private Joystick joystick;
    private Rigidbody rigid;

    private bool isMoving = false;

    private int MOVE_HASH = Animator.StringToHash("isMove");

    private float rotSpeed = 10f;

    private Vector3 moveDirection;

    private bool canMoving = false;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<Joystick>();
    }


    private void Start()
    {
        cam = Camera.main;

        canMoving = true;

        joystick.onMovingStick += OnStickMove;
        joystick.onMovedStick += OnEndStickMove;
    }
    
    private void OnStickMove(Vector2 stickVector)
    {
        if (!isMoving)
        {
            isMoving = true;
            animator.SetBool(MOVE_HASH, true);
        }

        // 이동
        moveDirection = cam.transform.TransformDirection(stickVector);
        moveDirection.y = 0f;
        moveDirection.Normalize();
        
        //transform.position += moveDirection * 5f * Time.deltaTime;

        // 회전
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotSpeed * Time.deltaTime);
    }


    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position + moveDirection * 5f * Time.deltaTime);
    }


    private void OnEndStickMove()
    {
        if (isMoving)
        {
            isMoving = false;
            animator.SetBool(MOVE_HASH, false);
            moveDirection = Vector3.zero;
        }
    }
}

