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


    // ĵ���� Ȱ��ȭ
    public virtual void Show()
    {
        canvas.enabled = true;
    }


    // ĵ���� ��Ȱ��ȭ
    public virtual void Hide()
    {
        canvas.enabled = false;
    }
}
