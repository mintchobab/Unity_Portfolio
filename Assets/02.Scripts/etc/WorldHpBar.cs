using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHpBar : MonoBehaviour
{
    // World�ϱ� ������ ���� ������ ���⼭ �˾Ƽ� ó���ϸ��
    // ��Ȯ�� ���ڰ� ��µ� �ʿ�� ����

    [SerializeField]
    private Image hpBar;

    private Canvas canvas;
    private Vector2 originSize;


    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;

        originSize = hpBar.rectTransform.sizeDelta;
    }


    // �ۼ�Ʈ�� �˷��ֱ�
    public void ChangeHpBar(float value)
    {
        hpBar.rectTransform.sizeDelta = new Vector2(originSize.x * value, originSize.y);
    }
}
