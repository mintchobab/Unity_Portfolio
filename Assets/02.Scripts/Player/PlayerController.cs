using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private int currentHp;

    private HpController hpController;
    private PlayerStat playerStat;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();
        hpController = GetComponent<HpController>();
    }


    private void Start()
    {
        currentHp = playerStat.FindStat(StatType.Hp).GetValue();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }


    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        // UI º¯°æ
        int maxHp = playerStat.FindStat(StatType.Hp).GetValue();        
        hpController.ChangeHpUI(currentHp, maxHp);

        if (currentHp <= 0)
            Debug.LogWarning("»ç¸Á");
    }
}
