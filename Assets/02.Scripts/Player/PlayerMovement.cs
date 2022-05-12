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
    private Coroutine moveByJoystick;
    private Joystick joystick;

    private bool isMoving = false;

    private int MOVE_HASH = Animator.StringToHash("isMove");

    private float rotSpeed = 10f;



    // 프로퍼티 없애기
    private bool canMoving = false;
    public bool CanMoving
    {
        get => canMoving;
        set
        {
            if (value)
            {
                if (moveByJoystick != null)
                    StopCoroutine(moveByJoystick);

                moveByJoystick = StartCoroutine(MoveByJoystick());
            }
            else
            {
                if (moveByJoystick != null)
                    StopCoroutine(moveByJoystick);
            }

            canMoving = value;
        }
    }


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

        CanMoving = true;
    }


    private IEnumerator MoveByJoystick()
    {
        while (true)
        {
            if (joystick.IsMoving)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    animator.SetBool(MOVE_HASH, true);
                }

                // 이동
                Vector3 direction = cam.transform.TransformDirection(joystick.StickVector);
                direction.y = 0f;
                direction.Normalize();
                transform.position += direction * 5f * Time.deltaTime;

                // 회전
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            }
            else
            {
                if (isMoving)
                {
                    isMoving = false;
                    animator.SetBool(MOVE_HASH, false);
                }
            }

            yield return null;
        }
    }
}

