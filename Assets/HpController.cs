using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    // 플레이어의 경우는 World 말고 캔버스에 있는 Hp도 있음
    private WorldHpBar worldHpBar;

    private void Awake()
    {
        worldHpBar = GetComponentInChildren<WorldHpBar>();
    }


    // Canvas HP가 있는 경우와 없는 경우가 있음
    // 1.있는 경우 => 플레이어, 보스 or 엘리트 몬스터
    // 2.없는 경우 => 일반 몬스터
    public void ChangeHpUI(int currentHp, int maxHp)
    {
        // 일단은 World만 바꾸도록 하자
        float value = currentHp / (float)maxHp;
        worldHpBar.ChangeHpBar(value);
    }
}
