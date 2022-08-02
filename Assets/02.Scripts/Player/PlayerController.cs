using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace lsy
{
    public class PlayerController : Singleton<PlayerController>, IHpHasable
    {
        [field: SerializeField]
        public Transform PlayerLookPosition { get; private set; }

        [SerializeField]
        private GameObject model;

        private Camera cam;
        private Animator anim;
        private Rigidbody rigid;
        private Joystick joystick;
        private NavMeshAgent navMeshAgent;
        private InteractChecker interactChecker;
        private HpController hpController;

        private Action onEndInteract;

        private RaycastHit slopeHit;
        private Vector3 moveDirection;

        private bool canMoving = true;
        private bool isMoving;
        private bool isAutoMoving;

        private float rotSpeed = 10f;

        private int hashSpeed           = Animator.StringToHash("speed");
        private int hashSuccessInteract = Animator.StringToHash("successInteract");
        private int hashFailInteract    = Animator.StringToHash("failInteract");
        private int hashEndInteract     = Animator.StringToHash("endInteract");
        private int hashTakeDamage      = Animator.StringToHash("takeDamage");
        private int hashDead            = Animator.StringToHash("dead");


        public EquipController EquipController { get; private set; }
        public PlayerStatController StatController { get; private set; }
        public PlayerCombatController CombatController { get; private set; }

        private QuestManager questManager => Managers.Instance.QuestManager;
        private MainUIController inputUIController => Managers.Instance.UIManager.MainUIController;
        private DialogueUIController dialogueController => Managers.Instance.UIManager.DialogueUIController;



        public override void Init()
        {
            cam = Camera.main;

            rigid                   = GetComponent<Rigidbody>();
            StatController          = GetComponent<PlayerStatController>();
            hpController            = GetComponent<HpController>();
            navMeshAgent            = GetComponent<NavMeshAgent>();
            EquipController         = GetComponent<EquipController>();
            CombatController        = GetComponent<PlayerCombatController>();
            anim                    = GetComponentInChildren<Animator>();
            interactChecker         = GetComponentInChildren<InteractChecker>();

            joystick = Managers.Instance.UIManager.MainUIController.GetComponentInChildren<Joystick>();
            navMeshAgent.updateRotation = false;

            
        }

        private void OnEnable()
        {
            joystick.onMovingStick += OnMovingStick;
            joystick.onMovedStick += OnMovedStick;

            hpController.onTakeDamage += OnTakeDamage;
            hpController.onDead += OnDead;

            StatController.onChangedStat += OnChangedStat;
        }


        private void OnDisable()
        {
            joystick.onMovingStick -= OnMovingStick;
            joystick.onMovedStick -= OnMovedStick;

            hpController.onTakeDamage -= OnTakeDamage;
            hpController.onDead -= OnDead;

            StatController.onChangedStat -= OnChangedStat;
        }



        private void FixedUpdate()
        {
            if (IsOnSlope())
            {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
            }          

            rigid.MovePosition(transform.position + moveDirection * 5f * Time.deltaTime);
        }



        // ���� üũ
        private bool IsOnSlope()
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out slopeHit, 5f))
            {
                if (slopeHit.normal != Vector3.up)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }



        private void OnMovingStick(Vector2 stickVector)
        {
            if (!canMoving)
                return;

            if (!isMoving)
            {
                isMoving = true;
                //anim.SetBool(hashIsMove, true);
            }

            anim.SetFloat(hashSpeed, 1);

            // �̵�
            moveDirection = cam.transform.TransformDirection(stickVector);
            moveDirection.y = 0f;
            moveDirection.Normalize();

            // ȸ��
            if (moveDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotSpeed * Time.deltaTime);
        }


        private void OnMovedStick()
        {
            anim.SetFloat(hashSpeed, 0f);

            if (isMoving)
            {
                isMoving = false;
                moveDirection = Vector3.zero;
            }
        }


        public IEnumerator AutoMove(Transform moveTarget, float targetDistance)
        {
            isMoving = false;
            isAutoMoving = true;

            navMeshAgent.enabled = true;
            //navMeshAgent.SetDestination(targetPosition);

            anim.SetFloat(hashSpeed, 1f);

            float distance = Vector3.Distance(transform.position, moveTarget.position);

            while (distance > targetDistance)
            {
                navMeshAgent.SetDestination(moveTarget.position);

                // �߰��� ������ ��
                if (isMoving)
                {
                    isAutoMoving = false;
                    navMeshAgent.enabled = false;
                    yield break;
                }

                // ȸ��
                Vector3 lookRotation = navMeshAgent.steeringTarget - transform.position;
                lookRotation.y = 0f;

                if (lookRotation != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), 10f * Time.deltaTime);

                distance = Vector3.Distance(transform.position, moveTarget.position);

                yield return null;
            }

            navMeshAgent.enabled = false;
            anim.SetFloat(hashSpeed, 0f);
        }


        // ĳ���� �� ���̰� �ϱ�
        private void ShowModel()
        {
            model.SetActive(true);
        }


        // ĳ���� �� ���߱�
        private void HideModel()
        {
            model.SetActive(false);
        }


        private void OnTakeDamage()
        {
            if (!CombatController.IsProcessingSkill)
            {
                anim.SetTrigger(hashTakeDamage);
            }
        }


        // �÷��̾ �׾��� ��
        private void OnDead()
        {
            anim.SetTrigger(hashDead);

            rigid.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            enabled = false;
        }


        // ������ �ٲ� ��
        private void OnChangedStat()
        {
            // ü���� �ٲ� ��
            hpController.ChangeMaxHp(GetMaxHp());

            // ���ݷ�, ���� ���浵 �����Ű��
            hpController.ChangeDeffensivePower(GetDefensivePower());
        }


        public int GetMaxHp()
        {
            return StatController.PlayerStat.GetAddedHp();
        }

        public int GetDefensivePower()
        {
            return StatController.PlayerStat.GetAddedDefensivePower();
        }



        #region Interact

        private WorldUIInteractGaugeCanvas gaugeCanvas;
        private Coroutine moveToInteractable;
        private Coroutine interactingCollection;
        private Coroutine endCollectingAnimation;

        private bool isCompleted;
        private bool isQuestProgressing;



        public void StartInteract(IInteractable interactable, Transform interactObj, Action endAction)
        {
            if (moveToInteractable != null)
                StopCoroutine(moveToInteractable);

            if (endAction != null)
                onEndInteract = () => endAction();

            moveToInteractable = StartCoroutine(MoveToInteractable(interactable, interactObj));
        }


        // �Ÿ�üũ�ؼ� �ڵ� �̵�
        private IEnumerator MoveToInteractable(IInteractable interactable, Transform interactObj)
        {
            yield return StartCoroutine(AutoMove(interactObj, interactable.GetInteractDistance()));

            if (!isAutoMoving)
            {
                yield break;
            }

            isAutoMoving = false;

            // Ȯ��
            if (interactable is InteractCollection)
            {
                StartInteractCollection((InteractCollection)interactable, interactObj);
            }
            else if (interactable is InteractNpc)
            {
                StartInteractNpc((InteractNpc)interactable, interactObj);
            }
        }



        // NPC�� ��ȣ�ۿ�
        private void StartInteractNpc(InteractNpc interactNpc, Transform interactObj)
        {
            Vector3 targetPos = interactObj.position + (interactObj.forward * 2) + (Vector3.up * 1.4f);
            Quaternion targetRot = interactObj.rotation * Quaternion.Euler(0f, 180f, 0f);

            CameraController.Instance.LookTarget(targetPos, targetRot);
            dialogueController.onDialougeClosed += CameraController.Instance.RestoreCamera;
            dialogueController.onDialougeClosed += ShowModel;
            dialogueController.onDialougeClosed += interactNpc.ChangeAnimationToIdle;

            if (interactNpc.MyQuest == null)
            {
                ShowDialogue(interactNpc);
            }
            else
            {
                ShowQuestDialogue(interactNpc);
            }
        }



        private void ShowDialogue(InteractNpc interactNpc)
        {
            List<string> dialogues = new List<string>();
            for (int i = 0; i < interactNpc.Npc.dialogues.Count; i++)
            {
                dialogues.Add(StringManager.GetLocalizedNpcDialogue(interactNpc.Npc.dialogues[i]));
            }

            dialogueController.SetInitializeInfo(false, interactNpc.NpcName, dialogues);
            dialogueController.Show(HideModel);
        }


        private void ShowQuestDialogue(InteractNpc interactNpc)
        {
            // ����Ʈ ����
            if (questManager.CurrentQuest == null)
            {
                ShowQuestStartDialogue(interactNpc);
            }
            else
            {
                // ����Ʈ ������
                if (!questManager.CurrentQuest.IsCompleted)
                {
                    ShowQuestProgressingDialogue(interactNpc);
                }
                // ����Ʈ �Ϸ�
                else
                {
                    ShowQuestCompleteDialogue(interactNpc);
                }
            }
        }



        // ����Ʈ�� ���� ������ ��ȭ
        private void ShowQuestStartDialogue(InteractNpc interactNpc)
        {
            List<string> dialouges = null;
            List<string> resultDialogues = new List<string>();

            dialouges = interactNpc.MyQuest.startDialogues;

            for (int i = 0; i < dialouges.Count; i++)
            {
                resultDialogues.Add(StringManager.GetLocalizedQuestDialogue(dialouges[i]));
            }

            dialogueController.SetAcceptButtonEvent(() =>
            {
                isQuestProgressing = true;
                questManager.TakeQuest(interactNpc.MyQuest);

                interactNpc.DestoryExclamationMark();
            });

            dialogueController.SetInitializeInfo(true, interactNpc.NpcName, resultDialogues);
            dialogueController.Show(HideModel);
        }


        // ����Ʈ ������ ������ ��ȭ
        private void ShowQuestProgressingDialogue(InteractNpc interactNpc)
        {
            List<string> dialouges = null;
            List<string> resultDialogues = new List<string>();

            dialouges = interactNpc.MyQuest.progressingDialogues;

            for (int i = 0; i < dialouges.Count; i++)
            {
                resultDialogues.Add(StringManager.GetLocalizedQuestDialogue(dialouges[i]));
            }

            dialogueController.SetInitializeInfo(false, interactNpc.NpcName, resultDialogues);
            dialogueController.Show(HideModel);
        }


        // ����Ʈ �Ϸ�� ������ ��ȭ
        private void ShowQuestCompleteDialogue(InteractNpc interactNpc)
        {
            List<string> dialouges = null;
            List<string> resultDialogues = new List<string>();

            dialouges = interactNpc.MyQuest.endDialogues;

            for (int i = 0; i < dialouges.Count; i++)
            {
                resultDialogues.Add(StringManager.GetLocalizedQuestDialogue(dialouges[i]));

                interactNpc.DestroyQuestionMark();
            }

            dialogueController.SetInitializeInfo(false, interactNpc.NpcName, resultDialogues);
            dialogueController.Show(HideModel);

            // �̰� ��� ���� �����غ���
            isQuestProgressing = false;
            questManager.CompleteQuest();
        }



        // �������� ��ȣ�ۿ�
        // �ڵ��̵� �߰��ϱ�.....
        private void StartInteractCollection(InteractCollection interactCollection, Transform interactObj)
        {
            isCompleted = false;
            canMoving = false;

            anim.SetTrigger(interactCollection.MyCollectionData.AnimationHash);
            EquipController.MakeTool(interactCollection.CollectionType);

            if (!gaugeCanvas)
                gaugeCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUIInteractGaugeCanvas>(ResourcePath.WorldInteractGaugeCanvas);

            gaugeCanvas.transform.position = interactObj.position + interactCollection.MyCollectionData.gaugePosition;
            gaugeCanvas.Successed += () => CollectingSuccessed(interactCollection, interactObj.position);
            gaugeCanvas.Failed += CollectingFailed;

            // �۾����� �ð� �ٸ����ϱ�????????????????????????????
            gaugeCanvas.StartProcess(0.8f, 5f, 8f);

            // ��ư �̹��� ����
            inputUIController.SetStopInteractButton(StopCollecting);

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            // �ڷ�ƾ ����
            if (interactingCollection != null)
                StopCoroutine(interactingCollection);

            interactingCollection = StartCoroutine(InteractingCollection(interactCollection.MyCollectionData, interactObj));
        }


        // ��ȣ�ۿ� ���� (�÷��̾� �ִϸ��̼� + �ڸ��̵� + ������Ʈ �ٶ󺸱�)
        private IEnumerator InteractingCollection(CollectionData collectionData, Transform target)
        {
            //  �̵� & ȸ��
            float time = 0f;
            float timeToMove = 0.1f;

            Vector3 direction = (transform.position - target.position).normalized;
            Vector3 startPos = transform.position;

            Vector3 targetPos = target.position + direction * collectionData.InteractDistance;

            targetPos.y = transform.position.y;
            direction = Quaternion.Euler(0f, 180, 0f) * direction;
            direction.y = 0;


            while (time < 1)
            {
                time += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(startPos, targetPos, time);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), time);
                yield return null;
            }
        }        


        // ��ȣ�ۿ� ����
        private void CollectingSuccessed(InteractCollection interactCollection, Vector3 targetPosition)
        {
            anim.SetTrigger(hashSuccessInteract);

            // ������ ȹ��
            Managers.Instance.InventoryManager.AddCountableItem(interactCollection.ItemId, 1);
            Managers.Instance.UIManager.SystemUIController.GetItem(interactCollection, targetPosition);
            CompletedCollecting();
        }


        // ��ȣ�ۿ� ����
        private void CollectingFailed()
        {
            anim.SetTrigger(hashFailInteract);
            CompletedCollecting();
        }


        // ��ȣ�ۿ� ����
        public void StopCollecting()
        {
            if (isCompleted)
                return;

            if (interactingCollection != null)
                StopCoroutine(interactingCollection);

            canMoving = true;

            anim.SetTrigger(hashEndInteract);

            EquipController.DestoryCurrentTool();
            interactChecker.RestartFindInteractable();

            if (gaugeCanvas != null)
            {
                Destroy(gaugeCanvas.gameObject);
                gaugeCanvas = null;
            }
        }


        // ��ȣ�ۿ� ����
        private void CompletedCollecting()
        {
            isCompleted = true;

            Destroy(gaugeCanvas.gameObject);
            gaugeCanvas = null;

            onEndInteract?.Invoke();
            onEndInteract = null;

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            endCollectingAnimation = StartCoroutine(EndCollectingAnimation());
        }

        
        // ��ȣ�ۿ� ��� �ִϸ��̼� ����
        private IEnumerator EndCollectingAnimation()
        {
            yield return new WaitForSeconds(ValueData.AfterInteractDelayTime);

            EquipController.DestoryCurrentTool();

            interactChecker.RestartFindInteractable();

            anim.SetTrigger(hashEndInteract);
            canMoving = true;
        }

        #endregion
    }
}
