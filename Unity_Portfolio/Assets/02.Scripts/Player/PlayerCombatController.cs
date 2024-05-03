using System;
using System.Collections;
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
        private PlayerEquipController equipController;
        private PlayerInteractChecker interactChecker;
        private Animator anim;
        private Transform targetMonster;

        private Coroutine startSkill;
        private Coroutine findTargetMonster;

        private bool isAutoMoving;

        private int monsterLayer;
        private int hashstartCombat = Animator.StringToHash("startCombat");

        private float overlapRange = 10f;

        private MainUIController inputUIController => Managers.Instance.UIManager.MainUIController;
        private CombatManager combatManager => Managers.Instance.CombatManager;

        public bool IsProcessingSkill { get; private set; }


        private void Awake()
        {
            playerController    = GetComponent<PlayerController>();
            statController      = GetComponent<PlayerStatController>();
            equipController     = GetComponent<PlayerEquipController>();
            anim                = GetComponentInChildren<Animator>();
            interactChecker     = GetComponentInChildren<PlayerInteractChecker>();

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
   

        public void OnStartCombat()
        {
            interactChecker.enabled = false;

            inputUIController.SetCombatReadyButton(() => equipController.Equip(OnEnquiped));

            if (findTargetMonster != null)
                StopCoroutine(findTargetMonster);

            findTargetMonster = StartCoroutine(FindTargetMonster());
        }


        private void OnEnquiped()
        {
            anim.SetTrigger(hashstartCombat);
            inputUIController.ActivateCombatButton();
        }


        private IEnumerator FindTargetMonster()
        {
            yield return null;

            while (IsProcessingSkill)
            {
                yield return null;
            }

            targetMonster = null;

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

                yield return new WaitForSeconds(0.1f);
            }
        }


        private void OnTargetDead()
        {
            circleController.HideCircle();

            if (findTargetMonster != null)
                StopCoroutine(findTargetMonster);

            findTargetMonster = StartCoroutine(FindTargetMonster());
        }


        public void OnClickSkillButton(PlayerSkill playerSkill, CombatButton combatButton)
        {
            if (IsProcessingSkill)
                return;

            if (playerSkill.IsCoolDown)
                return;

            if (!targetMonster)
                return;

            if (isAutoMoving)
                StopCoroutine(startSkill);

            startSkill = StartCoroutine(StartSkill(playerSkill, combatButton));
        }


        private IEnumerator StartSkill(PlayerSkill playerSkill, CombatButton combatButton)
        {
            isAutoMoving = true;
            yield return StartCoroutine(playerController.AutoMove(targetMonster, playerSkill.DistanceToTarget));
            isAutoMoving = false;

            Action onEnded = null;

            playerSkill.Activate();
            anim.SetTrigger(playerSkill.HashSkill);
            playerController.DisableCanMoving();

            if (combatButton is SkillButton)
            {
                SkillButton button = combatButton as SkillButton;
                float coolTime = playerSkill.SkillData.coolTime;
                button.StartCoroutine(button.FillImageCoolTime(coolTime));

                inputUIController.SizeUpSkillButtons();
                onEnded = () => inputUIController.SizeDownSkillButtons();
            }

            Vector3 dir = (targetMonster.position - transform.position).normalized;
            dir.y = 0f;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);


            StartCoroutine(CoolTime(playerSkill));
            StartCoroutine(ProcessingSkill(playerSkill, onEnded));

            yield return null;
        }


        private IEnumerator CoolTime(PlayerSkill playerSkill)
        {
            playerSkill.IsCoolDown = true;
            yield return new WaitForSeconds(playerSkill.SkillData.coolTime);
            playerSkill.IsCoolDown = false;
        }


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
                            float damage = UnityEngine.Random.Range(playerSkill.SkillData.minDamage, playerSkill.SkillData.maxDamage) * 0.01f;
                            damage *= statController.PlayerStat.GetAddedOffensivePower();

                            hpController.TakeDamage((int)damage);
                            playerSkill.onExecuteSkill?.Invoke(targetMonster);

                            Managers.Instance.SoundManager.Play("Hit", SoundType.SFX);

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
