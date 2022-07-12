using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheckInnerCollider : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> testList = new List<GameObject>();

    private SphereCollider coll;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            coll.enabled = false;
            coll.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            testList.Clear();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        testList.Add(other.gameObject);
    }

    

}
