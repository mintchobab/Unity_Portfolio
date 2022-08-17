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
        private PlayerInteractChecker interactChecker;
        private HpController hpController;

        private Action onStartInteract;
        private Action onEndInteract;

        private RaycastHit slopeHit;
        private Vector3 moveDirection;

        private bool canMoving = true;
        private bool isMoving;

        private int hashSpeed           = Animator.StringToHash("speed");
        private int hashSuccessInteract = Animator.StringToHash("successInteract");
        private int hashFailInteract    = Animator.StringToHash("failInteract");
        private int hashEndInteract     = Animator.StringToHash("endInteract");
        private int hashTakeDamage      = Animator.StringToHash("takeDamage");
        private int hashDead            = Animator.StringToHash("dead");

        private readonly float afterInteractDelay = 3f;

        private readonly float npcForwardDistance = 2f;
        private readonly float npcUpDistance = 1.4f;
        private float rotSpeed = 10f;

        public PlayerEquipController EquipController { get; private set; }
        public PlayerStatController StatController { get; private set; }
        public PlayerCombatController CombatController { get; private set; }

        private QuestManager questManager => Managers.Instance.QuestManager;
        private MainUIController inputUIController => Managers.Instance.UIManager.MainUIController;
        private DialogueUIController dialogueController => Managers.Instance.UIManager.DialogueUIController;
        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;


        public override void Init()
        {
            cam = Camera.main;

            rigid                   = GetComponent<Rigidbody>();
            StatController          = GetComponent<PlayerStatController>();
            hpController            = GetComponent<HpController>();
            navMeshAgent            = GetComponent<NavMeshAgent>();
            EquipController         = GetComponent<PlayerEquipController>();
            CombatController        = GetComponent<PlayerCombatController>();
            anim                    = GetComponentInChildren<Animator>();
            interactChecker         = GetComponentInChildren<PlayerInteractChecker>();

            joystick = Managers.Instance.UIManager.MainUIController.GetComponentInChildren<Joystick>();
            navMeshAgent.updateRotation = false;

            if (!gaugeCanvas)
                gaugeCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUIInteractGaugeCanvas>(ResourcePath.WorldInteractGaugeCanvas);
        }

        private void OnEnable()
        {
            joystick.onMovingStick += OnMovingStick;
            joystick.onMovedStick += OnMovedStick;

            hpController.onTakeDamage += OnTakeDamage;
            hpController.onDead += OnDead;

            StatController.onChangedStat += OnChangedStat;

            inventoryManager.onItemUsed += OnItemUsed;
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


        public void EnableCanMoving()
        {
            canMoving = true;
        }


        public void DisableCanMoving()
        {
            canMoving = false;
        }



        private void OnMovingStick(Vector2 stickVector)
        {
            if (!canMoving)
                return;

            if (!isMoving)
            {
                isMoving = true;
            }

            anim.SetFloat(hashSpeed, 1);

            moveDirection = cam.transform.TransformDirection(stickVector);
            moveDirection.y = 0f;
            moveDirection.Normalize();

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
            float distance = Vector3.Distance(transform.position, moveTarget.position);

            if (distance < targetDistance)
                yield break;

            isMoving = false;

            navMeshAgent.enabled = true;
            anim.SetFloat(hashSpeed, 1f);

            while (distance > targetDistance)
            {
                navMeshAgent.SetDestination(moveTarget.position);

                if (isMoving)
                {
                    navMeshAgent.enabled = false;
                    yield break;
                }

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


        private void ShowModel()
        {
            model.SetActive(true);
        }


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


        private void OnDead()
        {
            anim.SetTrigger(hashDead);

            rigid.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            enabled = false;
        }


        private void OnChangedStat()
        {
            hpController.ChangeMaxHp(GetMaxHp());
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


        private void OnItemUsed(int itemId, int itemIndex)
        {
            if (inventoryManager.ItemList[itemIndex].item.name.Equals("Item_Name_HpPotion"))
            {
                hpController.AddCurrentHp(100);
            }
        }



        #region Interact

        private WorldUIInteractGaugeCanvas gaugeCanvas;
        private Coroutine moveToInteractable;
        private Coroutine interactingCollection;
        private Coroutine endCollectingAnimation;

        private bool isCompleted;



        public void StartInteract(IInteractable interactable, Transform interactObj, Action startAction, Action endAction)
        {
            if (moveToInteractable != null)
                StopCoroutine(moveToInteractable);

            if (startAction != null)
                onStartInteract = () => startAction();

            if (endAction != null)
                onEndInteract = () => endAction();

            moveToInteractable = StartCoroutine(MoveToInteractable(interactable, interactObj));
        }


        private IEnumerator MoveToInteractable(IInteractable interactable, Transform interactObj)
        {
            yield return StartCoroutine(AutoMove(interactObj, interactable.GetInteractDistance()));

            onStartInteract?.Invoke();

            if (interactable is InteractCollection)
            {
                StartInteractCollection((InteractCollection)interactable, interactObj);
            }
            else if (interactable is InteractNpc)
            {
                StartInteractNpc((InteractNpc)interactable, interactObj);
            }
        }


        private void StartInteractNpc(InteractNpc interactNpc, Transform interactObj)
        {
            Vector3 targetPos = interactObj.position + (interactObj.forward * npcForwardDistance) + (Vector3.up * npcUpDistance);
            Quaternion targetRot = interactObj.rotation * Quaternion.Euler(0f, 180f, 0f);

            CameraController.Instance.LookNpc(targetPos, targetRot);
            dialogueController.onDialougeClosed += CameraController.Instance.RestoreCamera;
            dialogueController.onDialougeClosed += ShowModel;
            dialogueController.onDialougeClosed += interactNpc.OnDialougeClosed;

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
            if (questManager.CurrentQuest == null)
            {
                ShowQuestStartDialogue(interactNpc);
            }
            else
            {
                if (!questManager.CurrentQuest.IsCompleted)
                {
                    ShowQuestProgressingDialogue(interactNpc);
                }
                else
                {
                    ShowQuestCompleteDialogue(interactNpc);
                }
            }
        }


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
                questManager.TakeQuest(interactNpc.MyQuest);

                interactNpc.DestoryExclamationMark();
            });

            dialogueController.SetInitializeInfo(true, interactNpc.NpcName, resultDialogues);
            dialogueController.Show(HideModel);
        }


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

            questManager.CompleteQuest();
        }


        private void StartInteractCollection(InteractCollection interactCollection, Transform interactObj)
        {
            isCompleted = false;
            DisableCanMoving();

            anim.SetTrigger(interactCollection.MyCollectionData.AnimationHash);
            EquipController.MakeTool(interactCollection.CollectionType);

            gaugeCanvas.gameObject.SetActive(true);
            gaugeCanvas.transform.position = interactObj.position + interactCollection.MyCollectionData.gaugePosition;
            gaugeCanvas.Successed += () => CollectingSuccessed(interactCollection, interactObj.position);
            gaugeCanvas.Failed += CollectingFailed;
            gaugeCanvas.StartProcess(0.8f, 5f, 8f);

            inputUIController.SetStopInteractButton(StopCollecting);

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            if (interactingCollection != null)
                StopCoroutine(interactingCollection);

            Managers.Instance.SoundManager.Play(interactCollection.MyCollectionData.sfxName, SoundType.SFX_Long);

            interactingCollection = StartCoroutine(InteractingCollection(interactCollection.MyCollectionData, interactObj));
        }


        private IEnumerator InteractingCollection(CollectionData collectionData, Transform target)
        {
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


        private void CollectingSuccessed(InteractCollection interactCollection, Vector3 targetPosition)
        {
            anim.SetTrigger(hashSuccessInteract);

            Managers.Instance.InventoryManager.AddCountableItem(interactCollection.ItemId, 1);
            Managers.Instance.UIManager.SystemUIController.GetItem(interactCollection, targetPosition);

            Managers.Instance.SoundManager.Play("Success", SoundType.SFX);

            CompletedCollecting();
        }


        private void CollectingFailed()
        {
            anim.SetTrigger(hashFailInteract);
            Managers.Instance.SoundManager.Play("Fail", SoundType.SFX);

            CompletedCollecting();
        }


        public void StopCollecting()
        {
            if (isCompleted)
                return;

            if (interactingCollection != null)
                StopCoroutine(interactingCollection);

            EnableCanMoving();

            anim.SetTrigger(hashEndInteract);

            EquipController.DestoryCurrentTool();
            interactChecker.RestartFindInteractable();

            gaugeCanvas.gameObject.SetActive(false);
        }


        private void CompletedCollecting()
        {
            isCompleted = true;

            gaugeCanvas.gameObject.SetActive(false);

            onEndInteract?.Invoke();
            onEndInteract = null;

            Managers.Instance.SoundManager.Stop(SoundType.SFX_Long);

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            endCollectingAnimation = StartCoroutine(EndCollectingAnimation());
        }

        
        private IEnumerator EndCollectingAnimation()
        {
            yield return new WaitForSeconds(afterInteractDelay);

            EquipController.DestoryCurrentTool();

            interactChecker.RestartFindInteractable();

            anim.SetTrigger(hashEndInteract);
            EnableCanMoving();
        }

        #endregion
    }
}
