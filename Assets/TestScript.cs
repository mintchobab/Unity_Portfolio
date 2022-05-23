using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.LogWarning("Count : " + Managers.Instance.QuestManager.ProgressingList.Count);
        }
    }
}
