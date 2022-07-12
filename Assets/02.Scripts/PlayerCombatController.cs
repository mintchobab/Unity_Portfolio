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


        // ���۵ǰ� �Ÿ��� �ʹ� �־����� ����
        // ���Ͱ� ������ �ٽ� ��� ã��
        // ��� ������ ����
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
   

        // ������� ��ư ������ ��ų�� ����
        // ���� ����� Ÿ�� ã��...
        // Ÿ���� �ٲ�� ����???
        // 1.��ġ�ϸ� �ٲ� ---- �̰� ä���ұ�?????
        // 2.������ �ٲ�
        public void StartCombat()
        {
            // ���ͷ�Ʈ üũ ��������
            interactChecker.enabled = false;

            // ��� �������� ��ư ���� => �̺�Ʈ �߰��ϱ�
            inputUIController.SetCombatReadyButton(() => equipController.Equip(OnEnquiped));

            // ������ ã��
            Transform monsterTr = FindFirstTargetMonster();

            if (monsterTr)
            {
                MonsterBT monster = monsterTr.GetComponent<MonsterBT>();
                circleController.ShowCircle(monster.transform, Vector3.zero);
            }
        }


        // ��� ������ ����
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
