using System;
using UnityEngine;

public class SceneUI : MonoBehaviour
{
    protected Canvas canvas;


    protected virtual void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    // ĵ���� Ȱ��ȭ
    public virtual void Show(Action onShow = null)
    {
        canvas.enabled = true;

        onShow?.Invoke();
    }


    // ĵ���� ��Ȱ��ȭ
    public virtual void Hide(Action onHide = null)
    {
        canvas.enabled = false;

        onHide?.Invoke();
    }
}
