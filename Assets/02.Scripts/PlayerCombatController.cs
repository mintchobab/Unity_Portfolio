using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{


    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField]
        private TargetCircleController circleController;

        [SerializeField]
        private PlayerSkill normalAttack;

        [SerializeField]
        private PlayerSkill[] playerSkills;


        // 시작되고 거리가 너무 멀어지면 종료
        // 몬스터가 죽으면 다시 대상 찾기
        // 대상 없으면 종료
        public Action onStoppedCombat;
        private EquipController equipController;
        private InteractChecker interactChecker;
        private Animator anim;

        private bool isCombating;

        private int monsterLayer;
        private float overlapRange = 10f;

        private int hashstartCombat = Animator.StringToHash("startCombat");
        private int hashIsRun = Animator.StringToHash("isRun");

        private InputUIController inputUIController => Managers.Instance.UIManager.InputUIController;


        private void Awake()
        {
            equipController = GetComponent<EquipController>();
            anim = GetComponentInChildren<Animator>();
            interactChecker = GetComponentInChildren<InteractChecker>();

            monsterLayer = 1 << LayerMask.NameToLayer("Monster");
        }


        private void Start()
        {
            SetSkillButton();
        }



        private void SetSkillButton()
        {
            SkillData[] skillDatas = new SkillData[playerSkills.Length];

            for (int i = 0; i < skillDatas.Length; i++)
            {
                skillDatas[i] = playerSkills[i].SkillData;
            }

            inputUIController.ChangeSkillButtons(skillDatas);



        }
   

        // 장비장착 버튼 누르면 스킬로 변경
        // 가장 가까운 타겟 찾기...
        // 타겟이 바뀌는 경우는???
        // 1.터치하면 바뀜 ---- 이걸 채용할까?????
        // 2.죽으면 바뀜
        public void StartCombat()
        {
            // 인터렉트 체크 꺼버리기
            interactChecker.enabled = false;

            // 장비 장착으로 버튼 변경 => 이벤트 추가하기
            inputUIController.SetCombatReadyButton(() => equipController.Equip(OnEnquiped));

            // 상대방을 찾기
            Transform monsterTr = FindFirstTargetMonster();

            if (monsterTr)
            {
                MonsterBT monster = monsterTr.GetComponent<MonsterBT>();
                circleController.ShowCircle(monster.transform, Vector3.zero);
            }
        }


        // 장비가 장착된 이후
        private void OnEnquiped()
        {
            anim.SetTrigger(hashstartCombat);
            anim.SetBool(hashIsRun, false);
            inputUIController.ActivateCombatButton();
        }



        private Transform FindFirstTargetMonster()
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, overlapRange, monsterLayer);

            if (colls.Length > 0)
            {
                Transform target = colls[0].transform;
                float minDist = Vector3.SqrMagnitude(colls[0].transform.position - transform.position);

                for (int i = 1; i < colls.Length; i++)
                {
                    float dist = Vector3.SqrMagnitude(colls[i].transform.position - transform.position);

                    if (dist < minDist)
                    {
                        target = colls[i].transform;
                        minDist = dist;
                    }
                }
                return target;
            }
            return null;
        }


        public void StopCombat()
        {
            isCombating = false;
            onStoppedCombat?.Invoke();
        }
    }
}
