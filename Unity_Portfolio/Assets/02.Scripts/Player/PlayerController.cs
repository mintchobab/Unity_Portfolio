using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace lsy
{
    public partial class PlayerController : Singleton<PlayerController>, IHpHasable
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

        private QuestManager questManager => Managers.Instance.QuestManager;
        //private MainUIController inputUIController => Managers.Instance.UIManager.MainUIController;
        private DialogueUIController dialogueController => Managers.Instance.UIManager.DialogueUIController;
        private InventoryManager inventoryManager => Managers.Instance.InventoryManager;


        public override void Init()
        {
            cam = Camera.main;

            rigid                   = GetComponent<Rigidbody>();
            hpController            = GetComponent<HpController>();
            navMeshAgent            = GetComponent<NavMeshAgent>();
            anim                    = GetComponentInChildren<Animator>();
            interactChecker         = GetComponentInChildren<PlayerInteractChecker>();

            joystick = Managers.Instance.UIManager.MainUIController.GetComponentInChildren<Joystick>();
            navMeshAgent.updateRotation = false;

            if (!gaugeCanvas)
                gaugeCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUIInteractGaugeCanvas>(ResourcePath.WorldInteractGaugeCanvas);

            InitStat();
            InitCombat();
        }

        private void OnEnable()
        {
            joystick.onMovingStick += OnMovingStick;
            joystick.onMovedStick += OnMovedStick;

            hpController.onTakeDamage += OnTakeDamage;
            hpController.onDead += OnDead;

            inventoryManager.onItemUsed += OnItemUsed;
            inventoryManager.onAfterChangedEquipedItem += OnAfterChangedEquipedItem;

            OnEnableStat();
        }


        private void OnDisable()
        {
            joystick.onMovingStick -= OnMovingStick;
            joystick.onMovedStick -= OnMovedStick;

            hpController.onTakeDamage -= OnTakeDamage;
            hpController.onDead -= OnDead;

            inventoryManager.onItemUsed -= OnItemUsed;
            inventoryManager.onAfterChangedEquipedItem -= OnAfterChangedEquipedItem;

            OnDisableStat();
        }

        private void Start()
        {
            StartCombat();
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
            if (!IsProcessingSkill)
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


        // TODO 합치기?
        private void OnAfterChangedEquipedItem()
        {
            hpController.ChangeMaxHp(GetMaxHp());
            hpController.ChangeDeffensivePower(GetDefensivePower());
        }




        private void OnItemUsed(int itemId, int itemIndex)
        {
            // TODO : 확인해보기
            //if (inventoryManager.ItemList[itemIndex].item.name.Equals("Item_Name_HpPotion"))
            //{
            //    hpController.AddCurrentHp(100);
            //}
        }
    }
}
