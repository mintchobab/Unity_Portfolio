using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace lsy
{
    public class PlayerController : Singleton<PlayerController>, IDamageable
    {
        [field: SerializeField]
        public Transform PlayerLookPosition { get; private set; }

        [SerializeField]
        private GameObject model;

        private Camera cam;
        private Animator anim;
        private Rigidbody rigid;
        private Joystick joystick;
        private InteractChecker interactChecker;
        private NavMeshAgent navMeshAgent;

        private Coroutine autoMove;

        private RaycastHit slopeHit;
        private Vector3 moveDirection;

        private bool canMoving = true;
        private bool isMoving;
        private bool isAutoMoving;

        private int currentHp;
        private float rotSpeed = 10f;

        private int hashSpeed = Animator.StringToHash("speed");
        private int hashSuccessInteract = Animator.StringToHash("successInteract");
        private int hashFailInteract = Animator.StringToHash("failInteract");
        private int hashEndInteract = Animator.StringToHash("endInteract");        


        public EquipController EquipController { get; private set; }
        private QuestManager questManager => Managers.Instance.QuestManager;
        private InputUIController inputUIController => Managers.Instance.UIManager.InputUIController;
        private DialogueUIController dialogueController => Managers.Instance.UIManager.DialogueUIController;



        public override void Init()
        {
            cam = Camera.main;

            rigid = GetComponent<Rigidbody>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            EquipController = GetComponent<EquipController>();
            anim = GetComponentInChildren<Animator>();
            interactChecker = GetComponentInChildren<InteractChecker>();

            joystick = Managers.Instance.UIManager.InputUIController.GetComponentInChildren<Joystick>();

            joystick.StickMoveStart += OnStickMoveStart;
            joystick.StickMoving += OnStickMoving;
            joystick.StickMoveEnd += OnStickMoveEnd;
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



        private void OnStickMoveStart()
        {
            //anim.SetBool(hashIsRun, true);
        }


        private void OnStickMoving(Vector2 stickVector)
        {
            if (!canMoving)
                return;

            if (!isMoving)
            {
                isMoving = true;
                //anim.SetBool(hashIsMove, true);
            }

            anim.SetFloat(hashSpeed, 1);

            // 이동
            moveDirection = cam.transform.TransformDirection(stickVector);
            moveDirection.y = 0f;
            moveDirection.Normalize();

            // 회전
            if (moveDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotSpeed * Time.deltaTime);
        }


        private void OnStickMoveEnd()
        {
            //anim.SetBool(hashIsRun, false);
            anim.SetFloat(hashSpeed, 0f);

            if (isMoving)
            {
                isMoving = false;
                moveDirection = Vector3.zero;
            }
        }



        public void TakeDamage(int damage)
        {
            //currentHp -= damage;

            // UI 변경
            //int maxHp = playerStat.FindStat(StatType.Hp).GetValue();
            //hpController.ChangeHpUI(currentHp, maxHp);

            //if (currentHp <= 0)
            //    Debug.LogWarning("사망");
        }

        public bool CheckIsDead()
        {
            return false;
        }





        private IEnumerator AutoMove(Vector3 targetPosition, float targetDistance)
        {
            isMoving = false;
            isAutoMoving = true;

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(targetPosition);

            anim.SetFloat(hashSpeed, 1f);
            //anim.SetBool(hashIsRun, true);

            float distance = Vector3.Distance(transform.position, targetPosition);

            while (distance > targetDistance)
            {
                // 중간에 멈췄을 때
                if (isMoving)
                {
                    isAutoMoving = false;
                    navMeshAgent.enabled = false;
                    yield break;
                }

                distance = Vector3.Distance(transform.position, targetPosition);
                yield return null;
            }

            navMeshAgent.enabled = false;
            //anim.SetBool(hashIsRun, false);
            anim.SetFloat(hashSpeed, 0f);
        }


        // 캐릭터 모델 보이게 하기
        private void ShowModel()
        {
            model.SetActive(true);
        }


        // 캐릭터 모델 감추기
        private void HideModel()
        {
            model.SetActive(false);
        }

        



        #region Interact

        private WorldUIInteractGaugeCanvas gaugeCanvas;
        private Coroutine moveToInteractable;
        private Coroutine interactingCollection;
        private Coroutine endCollectingAnimation;

        private bool isCompleted;
        private bool isQuestProgressing;



        public void StartInteract(IInteractable interactable, Transform interactObj)
        {
            if (moveToInteractable != null)
                StopCoroutine(moveToInteractable);

            moveToInteractable = StartCoroutine(MoveToInteractable(interactable, interactObj));
        }


        // 거리체크해서 자동 이동
        // 이동 입력이 있으면 취소하기
        // 자동이동 기능이 필요하네..................
        private IEnumerator MoveToInteractable(IInteractable interactable, Transform interactObj)
        {
            yield return StartCoroutine(AutoMove(interactObj.position, interactable.GetInteractDistance()));

            if (!isAutoMoving)
            {
                yield break;
            }

            isAutoMoving = false;

            // 확인
            if (interactable is InteractCollection)
            {
                StartInteractCollection((InteractCollection)interactable, interactObj);
            }
            else if (interactable is InteractNpc)
            {
                StartInteractNpc((InteractNpc)interactable, interactObj);
            }
        }



        // NPC와 상호작용
        private void StartInteractNpc(InteractNpc interactNpc, Transform interactObj)
        {
            Vector3 targetPos = interactObj.position + (interactObj.forward * 2) + Vector3.up;
            Quaternion targetRot = interactObj.rotation * Quaternion.Euler(0f, 180f, 0f);

            CameraController.Instance.LookTarget(targetPos, targetRot);
            dialogueController.onDialougeClosed += CameraController.Instance.RestoreCamera;
            dialogueController.onDialougeClosed += ShowModel;

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
            // 퀘스트 시작
            if (questManager.CurrentQuest == null)
            {
                ShowQuestStartDialogue(interactNpc);
            }
            else
            {
                // 퀘스트 진행중
                if (!questManager.CurrentQuest.IsCompleted)
                {
                    ShowQuestProgressingDialogue(interactNpc);
                }
                // 퀘스트 완료
                else
                {
                    ShowQuestCompleteDialogue(interactNpc);
                }
            }
        }



        // 퀘스트가 시작 상태의 대화
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


        // 퀘스트 진행중 상태의 대화
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


        // 퀘스트 완료된 상태의 대화
        private void ShowQuestCompleteDialogue(InteractNpc interactNpc)
        {
            List<string> dialouges = null;
            List<string> resultDialogues = new List<string>();

            dialouges = interactNpc.MyQuest.endDialogues;

            for (int i = 0; i < dialouges.Count; i++)
            {
                resultDialogues.Add(StringManager.GetLocalizedQuestDialogue(dialouges[i]));
            }

            dialogueController.SetInitializeInfo(false, interactNpc.NpcName, resultDialogues);
            dialogueController.Show(HideModel);

            // 이거 어디서 할지 생각해보기
            isQuestProgressing = false;
            questManager.CompleteQuest();
        }



        // 수집물과 상호작용
        // 자동이동 추가하기.....
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

            // 작업별로 시간 다르게하기????????????????????????????
            gaugeCanvas.StartProcess(0.8f, 5f, 8f);

            // 버튼 이미지 변경
            inputUIController.SetStopInteractButton(StopCollecting);

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            // 코루틴 실행
            if (interactingCollection != null)
                StopCoroutine(interactingCollection);

            interactingCollection = StartCoroutine(InteractingCollection(interactCollection.MyCollectionData, interactObj));
        }


        // 상호작용 동작 (플레이어 애니메이션 + 자리이동 + 오브젝트 바라보기)
        private IEnumerator InteractingCollection(CollectionData collectinData, Transform target)
        {
            //  이동 & 회전
            float time = 0f;
            float timeToMove = 0.1f;

            Vector3 direction = (transform.position - target.position).normalized;
            Vector3 startPos = transform.position;
            Vector3 targetPos = target.position + direction * collectinData.InteractDistance;

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


        // 상호작용 성공
        private void CollectingSuccessed(InteractCollection interactCollection, Vector3 targetPosition)
        {
            anim.SetTrigger(hashSuccessInteract);

            // 아이템 획득
            Managers.Instance.InventoryManager.AddCountableItem(interactCollection.ItemId, 1);
            Managers.Instance.UIManager.SystemUIController.GetItem(interactCollection, targetPosition);
            CompletedCollecting();
        }


        // 상호작용 실패
        private void CollectingFailed()
        {
            anim.SetTrigger(hashFailInteract);
            CompletedCollecting();
        }


        // 상호작용 정지
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


        // 상호작용 종료
        private void CompletedCollecting()
        {
            isCompleted = true;

            Destroy(gaugeCanvas.gameObject);
            gaugeCanvas = null;

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            endCollectingAnimation = StartCoroutine(EndCollectingAnimation());
        }

        
        // 상호작용 결과 애니메이션 종료
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
