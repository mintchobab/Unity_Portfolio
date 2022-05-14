using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        UpdateBillboard();
    }

    private void UpdateBillboard()
    {
        transform.rotation = cam.transform.rotation;
        //Vector3 direction = transform.position - cam.transform.position;
        //transform.rotation = Quaternion.LookRotation(direction);
    }
}
