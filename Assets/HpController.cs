using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    // �÷��̾��� ���� World ���� ĵ������ �ִ� Hp�� ����
    private WorldHpBar worldHpBar;

    private void Awake()
    {
        worldHpBar = GetComponentInChildren<WorldHpBar>();
    }


    // Canvas HP�� �ִ� ���� ���� ��찡 ����
    // 1.�ִ� ��� => �÷��̾�, ���� or ����Ʈ ����
    // 2.���� ��� => �Ϲ� ����
    public void ChangeHpUI(int currentHp, int maxHp)
    {
        // �ϴ��� World�� �ٲٵ��� ����
        float value = currentHp / (float)maxHp;
        worldHpBar.ChangeHpBar(value);
    }
}
