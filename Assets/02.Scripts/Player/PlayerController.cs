using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private int currentHp;

    private HpController hpController;
    private PlayerStat playerStat;
    private Animator anim;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();
        hpController = GetComponent<HpController>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        currentHp = playerStat.FindStat(StatType.Hp).GetValue();
    }


    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        // UI 변경
        int maxHp = playerStat.FindStat(StatType.Hp).GetValue();        
        hpController.ChangeHpUI(currentHp, maxHp);

        if (currentHp <= 0)
            Debug.LogWarning("사망");
    }

    public void TestAttack()
    {
        anim.SetTrigger("attack");
    }




    // 나무베기 테스트

    public void StartChop()
    {
        anim.SetTrigger("chop");
        StartCoroutine(ChopWood());
    }

    // 테스트 3초
    private IEnumerator ChopWood()
    {
        yield return new WaitForSeconds(3.0f);
        EndChop();
    }

    private void EndChop()
    {
        anim.SetTrigger("isFinishChop");
    }
}
