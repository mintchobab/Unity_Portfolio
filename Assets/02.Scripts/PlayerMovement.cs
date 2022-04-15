using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Camera cam;
    private NavMeshAgent navAgent;

    public Transform testPosition1;
    public Transform testPosition2;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }


    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            PrintMousePosition();
        }
    }


    private void PrintMousePosition()
    {
        // 마우스가 UI에서 클릭 or 드래그라면
        // 좀더 상황 세분화 하기
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            navAgent.destination = hit.point;
        }
    }
}

