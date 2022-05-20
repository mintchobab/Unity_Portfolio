using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHpBar : MonoBehaviour
{
    // World니까 비율만 전달 받으면 여기서 알아서 처리하면됨
    // 정확한 숫자가 출력될 필요는 없음

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


    // 퍼센트로 알려주기
    public void ChangeHpBar(float value)
    {
        hpBar.rectTransform.sizeDelta = new Vector2(originSize.x * value, originSize.y);
    }
}
