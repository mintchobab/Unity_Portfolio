using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float distance;

    private Transform player;
    private Quaternion originRotation;

    private Coroutine rotateCamera;

    private Vector3 prevPos;
    private Vector3 testRot;

    private float mouseDragDist;




    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        originRotation = transform.rotation;
    }


    private void Update()
    {
        // 테스트용...
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (rotateCamera != null)
                StopCoroutine(rotateCamera);

            rotateCamera = StartCoroutine(RotateCamera());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (rotateCamera != null)
                StopCoroutine(rotateCamera);
        }

        Zoom();
    }


    private IEnumerator RotateCamera()
    {
        prevPos = Input.mousePosition;

        while (true)
        {
            Vector3 newPos = Input.mousePosition;
            Vector3 dist = newPos - prevPos;

            mouseDragDist += dist.x;

            testRot = new Vector3(0f, mouseDragDist, 0f);
            prevPos = newPos;
            yield return null;
        }
    }


    private void Zoom()
    {
        distance -= Input.mouseScrollDelta.y;
        distance = Mathf.Clamp(distance, 7f, 12f);
    }




    void LateUpdate()
    {
        Vector3 direction = originRotation * Vector3.forward;
        direction = Quaternion.Euler(testRot) * direction;
        direction.Normalize();

        transform.position = (player.transform.position + new Vector3(0f, 0.8f, 0f)) - direction * distance;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
