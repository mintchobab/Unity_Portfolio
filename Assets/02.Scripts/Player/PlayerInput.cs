using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Action clickAction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickAction?.Invoke();
        }
    }

    public void AddClickAction(Action action)
    {
        clickAction += action;
    }

}
