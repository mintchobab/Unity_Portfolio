using System;
using UnityEngine;

public class SceneUI : MonoBehaviour
{
    protected Canvas canvas;


    protected virtual void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
    }


    public virtual void Show(Action onShow = null)
    {
        canvas.enabled = true;

        onShow?.Invoke();
    }


    public virtual void Hide(Action onHide = null)
    {
        canvas.enabled = false;

        onHide?.Invoke();
    }
}
