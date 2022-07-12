using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lsy;

public class DrawGizmoMonsterFOV : MonoBehaviour
{
    [SerializeField]
    private AttackMonsterBT monsterBT;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterBT.FovRange);
    }
}
