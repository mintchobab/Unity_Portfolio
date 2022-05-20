using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUI : MonoBehaviour
{
    protected Canvas canvas;

    protected virtual void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
    }


    // 캔버스 활성화
    public virtual void Show()
    {
        canvas.enabled = true;
    }


    // 캔버스 비활성화
    public virtual void Hide()
    {
        canvas.enabled = false;
    }
}
