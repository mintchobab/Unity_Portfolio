using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestCamera : MonoBehaviour
{
    public Transform cam;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            cam.transform.DOShakePosition(10f, 0.1f);
        }    
    }
}
