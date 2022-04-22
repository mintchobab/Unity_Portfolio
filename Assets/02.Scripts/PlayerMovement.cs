using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform testBody;

    [SerializeField]
    private Transform testDestination;


    private Camera cam;
    private NavMeshAgent navAgent;
    private Animator animator;
    private Coroutine moveToMousePosition;

    private int MOVE_HASH = Animator.StringToHash("isMove");

    private float rotSpeed = 10f;


    private bool canMoving = false;
    public bool CanMoving
    {
        get => canMoving;
        set
        {
            if (value)
            {
                if (moveToMousePosition != null)
                    StopCoroutine(moveToMousePosition);

                moveToMousePosition = StartCoroutine(MoveToMousePosition());
                
            }
            else
            {
                if (moveToMousePosition != null)
                    StopCoroutine(moveToMousePosition);
            }

            canMoving = value;
        }
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        cam = Camera.main;
        navAgent.updateRotation = false;

        CanMoving = true;
    }    


    private IEnumerator MoveToMousePosition()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // ****** 바닥만 인식할 수 있도록하기*****
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        navAgent.destination = hit.point;

                        testDestination.position = hit.point;
                    }
                }                
            }

            if (navAgent.remainingDistance < 0.1f)
            {
                animator.SetBool(MOVE_HASH, false);
            }
            else
            {
                animator.SetBool(MOVE_HASH, true);
                RotateToTarget();
            }            

            yield return null;
        }        
    }


    private void RotateToTarget()
    {
        Vector3 direction = navAgent.destination - transform.position;
        Quaternion lookTarget = Quaternion.LookRotation(direction);

        Quaternion nextRot = Quaternion.Slerp(testBody.rotation, lookTarget, rotSpeed * Time.deltaTime);
        nextRot.x = 0f;
        nextRot.z = 0f;
        testBody.rotation = nextRot;
    }
}

