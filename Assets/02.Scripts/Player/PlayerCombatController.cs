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


        public Action onStoppedCombat;

        private PlayerController playerController;
        private PlayerStatController statController;
        private EquipController equipController;
        private InteractChecker interactChecker;
        private Animator anim;
        private Transform targetMonster;

        private Coroutine startSkill;
        private Coroutine findTargetMonster;

        private bool isAutoMoving;

        private int monsterLayer;
        private float overlapRange = 10f;
        private int hashstartCombat = Animator.StringToHash("startCombat");

        private MainUIController inputUIController => Managers.Instance.UIManager.MainUIController;
        private CombatManager combatManager => Managers.Instance.CombatManager;

        public bool IsProcessingSkill { get; private set; }



        private void Awake()
        {
            playerController    = GetComponent<PlayerController>();
            statController      = GetComponent<PlayerStatController>();
            equipController     = GetComponent<EquipController>();
            anim                = GetComponentInChildren<Animator>();
            interactChecker     = GetComponentInChildren<InteractChecker>();

            monsterLayer = 1 << LayerMask.NameToLayer("Monster");

            combatManager.onStartedCombat += OnStartCombat;
        }

      
        private void Start()
        {
            SetSkillButton();
        }



        private void SetSkillButton()
        {
            inputUIController.ChangeSkillButtons(normalAttack, playerSkills, OnClickSkillButton);
        }
   

        // 전투 시작
        public void OnStartCombat()
        {
            interactChecker.enabled = false;

            // 장비 장착으로 버튼 변경 => 이벤트 추가하기
            inputUIController.SetCombatReadyButton(() => equipController.Equip(OnEnquiped));

            if (findTargetMonster != null)
                StopCoroutine(findTargetMonster);

            findTargetMonster = StartCoroutine(FindTargetMonster());
        }



        // 장비가 장착된 이후
        private void OnEnquiped()
        {
            anim.SetTrigger(hashstartCombat);
            inputUIController.ActivateCombatButton();
        }



        // 공격대상 찾기
        private IEnumerator FindTargetMonster()
        {
            yield return null;

            while (true)
            {
                Collider[] colls = Physics.OverlapSphere(transform.position, overlapRange, monsterLayer);

                if (colls.Length > 0)
                {
                    Transform target = colls[0].transform;
                    float minDist = Vector3.SqrMagnitude(colls[0].transform.position - transform.position);
                    int index = 0;

                    for (int i = 1; i < colls.Length; i++)
                    {
                        float dist = Vector3.SqrMagnitude(colls[i].transform.position - transform.position);

                        if (dist < minDist)
                        {
                            target = colls[i].transform;
                            minDist = dist;
                            index = i;
                        }
                    }

                    targetMonster = target;
                    targetMonster.GetComponent<HpController>().onDead += OnTargetDead;

                    Vector3 center = colls[index].bounds.center;
                    Vector3 addedPosition = center - targetMonster.position;

                    circleController.ShowCircle(targetMonster, addedPosition);

                    yield break;
                }

                yield return null;
            }
        }



        // 타겟이 죽었을 때
        private void OnTargetDead()
        {
            circleController.HideCircle();

            if (findTargetMonster != null)
                StopCoroutine(findTargetMonster);

            findTargetMonster = StartCoroutine(FindTargetMonster());
        }



        // 스킬 버튼들을 눌렀을 때
        public void OnClickSkillButton(PlayerSkill playerSkill, CombatButton combatButton)
        {
            if (IsProcessingSkill)
                return;

            if (playerSkill.IsCoolDown)
                return;

            if (isAutoMoving)
                StopCoroutine(startSkill);

            startSkill = StartCoroutine(StartSkill(playerSkill, combatButton));
        }


        

        // AutoMove 기능 추가하기
        // 1. AutoMove가 되면 일정 거리까지 달려가서 스킬쓰기
        // 2. 달려가는 도중에 다른 스킬쓰면??? => 기존 스킬 취소, 눌린 스킬 사용
        private IEnumerator StartSkill(PlayerSkill playerSkill, CombatButton combatButton)
        {
            isAutoMoving = true;
            yield return StartCoroutine(playerController.AutoMove(targetMonster, playerSkill.DistanceToTarget));
            isAutoMoving = false;

            Action onEnded = null;

            playerSkill.Activate();
            anim.SetTrigger(playerSkill.HashSkill);
            playerController.DisableCanMoving();

            // 스킬
            if (combatButton is SkillButton)
            {
                SkillButton button = combatButton as SkillButton;
                float coolTime = playerSkill.SkillData.coolTime;
                button.StartCoroutine(button.FillImageCoolTime(coolTime));

                inputUIController.SizeUpSkillButtons();
                onEnded = () => inputUIController.SizeDownSkillButtons();
            }

            // 타겟 바라보기
            Vector3 dir = (targetMonster.position - transform.position).normalized;
            dir.y = 0f;
            transform.rotation = Quaternion.LookRotation(dir);

            StartCoroutine(CoolTime(playerSkill));
            StartCoroutine(ProcessingSkill(playerSkill, onEnded));

            yield return null;
        }



        // 스킬 쿨타임 체크
        private IEnumerator CoolTime(PlayerSkill playerSkill)
        {
            playerSkill.IsCoolDown = true;
            yield return new WaitForSeconds(playerSkill.SkillData.coolTime);
            yield return new WaitForSeconds(3f);
            playerSkill.IsCoolDown = false;
        }


        // 스킬 발동
        private IEnumerator ProcessingSkill(PlayerSkill playerSkill, Action onEnded)
        {
            IsProcessingSkill = true;

            yield return null;

            float[] damageApplyPercents = playerSkill.damageApplyPercents;
            int count = 0;

            float elapsedTime = 0f;
            float length = anim.GetCurrentAnimatorStateInfo(0).length;

            while (elapsedTime < length)
            {
                if (count < damageApplyPercents.Length)
                {
                    if (damageApplyPercents[count] < elapsedTime / length)
                    {
                        HpController hpController = targetMonster.GetComponent<HpController>();

                        if (hpController != null)
                        {
                            // ★★★ 밀기 테스트용 ★★★

                            //Vector3 dir = targetMonster.transform.position - transform.position;
                            //targetMonster.GetComponent<MonsterBT>().Pushed(dir, 1f);

                            // ★★★ 밀기 테스트용 ★★★

                            // 밀리는 애니메이션 때문에 데미지를 더 늦게 적용
                            float damage = UnityEngine.Random.Range(playerSkill.SkillData.minDamage, playerSkill.SkillData.maxDamage) * 0.01f;
                            damage *= statController.PlayerStat.GetAddedOffensivePower();

                            hpController.TakeDamage((int)damage);
                            playerSkill.onExecuteSkill?.Invoke(targetMonster);

                            Time.timeScale = 0.1f;
                            yield return new WaitForSecondsRealtime(0.12f);
                            Time.timeScale = 1f;


                            CameraController.Instance.StartShaking(0.35f, 0.1f);
                            count++;
                        }                       
                    }
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            onEnded?.Invoke();

            IsProcessingSkill = false;
            playerSkill.Deactivate();
            playerController.EnableCanMoving();
        }
    }
}
