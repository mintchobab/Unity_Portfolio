using System;
using UnityEngine;

public class SceneUI : MonoBehaviour
{
    protected Canvas canvas;


    protected virtual void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    // 캔버스 활성화
    public virtual void Show(Action onShow = null)
    {
        canvas.enabled = true;

        onShow?.Invoke();
    }


    // 캔버스 비활성화
    public virtual void Hide(Action onHide = null)
    {
        canvas.enabled = false;

        onHide?.Invoke();
    }
}
