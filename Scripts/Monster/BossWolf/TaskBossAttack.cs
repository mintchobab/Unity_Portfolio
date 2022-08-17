using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

namespace lsy
{
    public class TaskBossAttack : Node
    {
        private enum AttackType { Default, Normal, CircleAttack, SectorAttack }

        private HpController hpController;
        private BossWolfBT monster;
        private Transform lastTarget;

        private AttackType attackType;

        private bool isWating = false;
        private bool isSkilling = false;

        private int hashWaitSkill = Animator.StringToHash("waitSkill");
        private int hashSkillAttack = Animator.StringToHash("skillAttack");

        private float attackCounter = 0f;
        private float waitingCounter = 0f;
        private float attackTime = 0f;
        private float skillWaitTime = 2f;        


        public TaskBossAttack(BossWolfBT monster)
        {
            this.monster = monster;
        }


        private void Initialize()
        {
            isSkilling = false;
            isWating = false;

            monster.IsAttacking = false;

            attackCounter = 0f;
            waitingCounter = 0f;

            monster.SkillIndicator.gameObject.SetActive(false);
        }


        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != lastTarget)
            {
                hpController = target.GetComponent<HpController>();
                lastTarget = target;
            }

            if (!monster.IsAttacking)
            {
                monster.IsAttacking = true;

                ChoiceAttackType();

                Vector3 lookDir = (target.position - monster.transform.position).normalized;
                lookDir.y = 0f;
                monster.transform.rotation = Quaternion.LookRotation(lookDir);

                state = NodeState.Success;
                return state;
            }
            

            attackTime = monster.Anim.GetCurrentAnimatorStateInfo(0).length;
            attackCounter += Time.deltaTime;

            if (isWating)
            {
                waitingCounter += Time.deltaTime;

                if (waitingCounter >= monster.AttackDelay)
                {
                    Initialize();

                    monster.Anim.SetFloat(monster.HashMoveSpeed, 0f);

                    state = NodeState.Success;
                    return state;
                }

                state = NodeState.Success;
                return state;
            }


            if (attackType == AttackType.Normal)
            {
                state = NormalAttack();
            }
            else
            {
                state = SkillAttack();
            }

            return state;
        }


        private NodeState NormalAttack()
        {
            if (attackCounter >= monster.AttackPercent * attackTime)
            {
                isWating = true;
                hpController.TakeDamage(monster.MonsterStat.offensivePower);

                if (hpController.IsDead)
                {
                    ClearData("target");
                    monster.Anim.SetFloat(monster.HashMoveSpeed, 0f);

                    state = NodeState.Success;
                    return state;
                }
            }

            state = NodeState.Success;
            return state;
        }


        private NodeState SkillAttack()
        {
            if (!isSkilling)
            {
                if (!monster.SkillIndicator.gameObject.activeSelf)
                    monster.SkillIndicator.gameObject.SetActive(true);

                monster.SkillIndicatorMr.material.SetFloat("_Duration", attackCounter / skillWaitTime);

                if (attackCounter >= skillWaitTime)
                {
                    isSkilling = true;
                    attackCounter = 0f;

                    monster.Anim.SetTrigger(hashSkillAttack);
                }
            }
            else
            {
                if (attackCounter >= monster.AttackPercent * attackTime)
                {
                    isWating = true;
                    monster.Anim.SetFloat(monster.HashMoveSpeed, 0f);
                    monster.SkillIndicator.gameObject.SetActive(false);

                    // 원형 범위 공격
                    if (attackType == AttackType.CircleAttack)
                    {
                        Collider[] colls = Physics.OverlapSphere(monster.transform.position, monster.CircularSkillRadius, monster.PlayerLayerMask);

                        if (colls.Length > 0)
                        {
                            if (colls[0].transform.Equals(lastTarget))
                            {
                                hpController.TakeDamage(monster.MonsterStat.offensivePower);
                            }
                        }
                    }
                    // 부채꼴 범위 공격
                    else
                    {
                        Vector3 dir = (lastTarget.position - monster.transform.position);

                        if (dir.magnitude <= monster.CircularSkillRadius)
                        {
                            float dot = Vector3.Dot(dir.normalized, monster.transform.forward);
                            float theta = Mathf.Acos(dot);
                            float degree = Mathf.Rad2Deg * theta;

                            if (degree <= monster.SectorAngle * 0.5f)
                            {
                                hpController.TakeDamage(monster.MonsterStat.offensivePower);
                            }
                        }                        
                    }                    

                    if (hpController.IsDead)
                    {
                        ClearData("target");
                        state = NodeState.Success;
                        return state;
                    }
                }
            }

            state = NodeState.Success;
            return state;
        }


        private void ChoiceAttackType()
        {
            int ran = Random.Range(0, 3);

            if (ran == 0)
            {
                attackType = AttackType.Normal;
                monster.Anim.SetTrigger(monster.HashAtack);
            }
            else if (ran == 1)
            {
                attackType = AttackType.CircleAttack;
                monster.Anim.SetTrigger(hashWaitSkill);

                monster.SkillIndicatorMr.material.SetInt("_Sector", 0);
            }
            else
            {
                attackType = AttackType.SectorAttack;
                monster.Anim.SetTrigger(hashWaitSkill);

                monster.SkillIndicatorMr.material.SetInt("_Sector", 1);
                monster.SkillIndicatorMr.material.SetFloat("_Angle", monster.SectorAngle);
            }
        }
    }
}
