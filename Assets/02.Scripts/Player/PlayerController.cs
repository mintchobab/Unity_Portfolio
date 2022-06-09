using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody rigid;
        private Animator anim;
        private Camera cam;
        private Joystick joystick;
        private PlayerEquipController equipController;
        private PlayerInteractChecker interactChecker;
        

        private Vector3 moveDirection;

        private bool canMoving = true;
        private bool isMoving;

        private int currentHp;
        private float rotSpeed = 10f;

        private int hashIsStickMove = Animator.StringToHash("isStickMove");
        private int hashSuccessInteract = Animator.StringToHash("successInteract");
        private int hashFailInteract = Animator.StringToHash("failInteract");
        private int hashEndInteract = Animator.StringToHash("endInteract");


        private BillboardUIController billboardController => Managers.Instance.UIManager.BillboardUIController;
        private InputUIController inputUIController => Managers.Instance.UIManager.InputUIController;



        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            equipController = GetComponent<PlayerEquipController>();
            anim = GetComponentInChildren<Animator>();
            interactChecker = GetComponentInChildren<PlayerInteractChecker>();
            joystick = Managers.Instance.UIManager.InputUIController.GetComponentInChildren<Joystick>();

            cam = Camera.main;

            joystick.StickMoveStart += OnStickMoveStart;
            joystick.StickMoving += OnStickMoving;
            joystick.StickMoveEnd += OnStickMoveEnd;
        }


        private void FixedUpdate()
        {
            rigid.MovePosition(transform.position + moveDirection * 5f * Time.deltaTime);
        }


        private void OnStickMoveStart()
        {
            anim.SetBool(hashIsStickMove, true);
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

            // �̵�
            moveDirection = cam.transform.TransformDirection(stickVector);
            moveDirection.y = 0f;
            moveDirection.Normalize();

            // ȸ��
            if (moveDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotSpeed * Time.deltaTime);
        }


        private void OnStickMoveEnd()
        {
            anim.SetBool(hashIsStickMove, false);

            if (isMoving)
            {
                isMoving = false;
                //anim.SetBool(hashIsMove, false);
                moveDirection = Vector3.zero;
            }
        }



        public void TakeDamage(int damage)
        {
            currentHp -= damage;

            // UI ����
            //int maxHp = playerStat.FindStat(StatType.Hp).GetValue();        
            //hpController.ChangeHpUI(currentHp, maxHp);

            if (currentHp <= 0)
                Debug.LogWarning("���");
        }




        #region Interact

        private InteractGauge currentGauge;
        private Coroutine interacting;
        private Coroutine endInteractAnimation;

        private bool isCompleted;


        // ��ȣ�ۿ� ����
        public void StartInteract(InteractData interactData, Transform interactObj)
        {
            isCompleted = false;
            canMoving = false;

            anim.SetTrigger(interactData.StartHash);
            equipController.MakeTool(interactData.InteractType, false);

            // UI ����
            currentGauge = Managers.Instance.ResourceManager.Instantiate<InteractGauge>(ResourcePath.InteractGauge, billboardController.transform);
            billboardController.AddTarget(currentGauge.gameObject, interactObj, new Vector3(0f, 1f, 0f));

            currentGauge.Successed += InteractSuccessed;
            currentGauge.Failed += InteractFailed;

            // �۾����� �ð� �ٸ����ϱ�????????????????????????????
            currentGauge.StartProcess(0.8f, 5f, 8f);

            // ��ư �̹��� ����
            inputUIController.SetStopInteractButton(StopInteracting);

            if (endInteractAnimation != null)
                StopCoroutine(endInteractAnimation);

            // �ڷ�ƾ ����
            if (interacting != null)
                StopCoroutine(interacting);

            interacting = StartCoroutine(Interacting(interactData, interactObj));
        }



        // ��ȣ�ۿ� ���� (�ִϸ��̼� + �ڸ��̵� + ������Ʈ �ٶ󺸱�)
        private IEnumerator Interacting(InteractData interactData, Transform target)
        {
            //  �̵� & ȸ��
            float time = 0f;
            float timeToMove = 0.1f;

            Vector3 startPos = transform.position;
            Vector3 posistion = target.position + ((transform.position - target.position).normalized * interactData.InteractDistance);
            posistion.y = 0f;

            Vector3 lookVector = target.position - transform.position;
            lookVector.y = 0f;

            transform.rotation = Quaternion.LookRotation(lookVector);

            while (time < 1)
            {
                time += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(startPos, posistion, time);
                yield return null;
            }
        }
        


        // ��ȣ�ۿ� ����
        private void InteractSuccessed()
        {
            anim.SetTrigger(hashSuccessInteract);
            InteractCompleted();
        }


        // ��ȣ�ۿ� ����
        private void InteractFailed()
        {
            anim.SetTrigger(hashFailInteract);
            InteractCompleted();
        }


        // ��ȣ�ۿ� ����
        public void StopInteracting()
        {
            if (isCompleted)
                return;

            if (interacting != null)
                StopCoroutine(interacting);

            canMoving = true;

            anim.SetTrigger(hashEndInteract);

            equipController.DestoryCurrentTool();
            interactChecker.StartCheckInteractTarget();

            if (currentGauge != null)
            {
                billboardController.RemoveTarget(currentGauge.gameObject);
                Destroy(currentGauge.gameObject);
                currentGauge = null;
            }
        }


        // ��ȣ�ۿ� ����
        private void InteractCompleted()
        {
            isCompleted = true;

            equipController.DestoryCurrentTool();            

            billboardController.RemoveTarget(currentGauge.gameObject);
            Destroy(currentGauge.gameObject);
            currentGauge = null;

            if (endInteractAnimation != null)
                StopCoroutine(endInteractAnimation);

            endInteractAnimation = StartCoroutine(EndInteractAnimation());
        }

        
        // ��ȣ�ۿ� ��� �ִϸ��̼� ����

        private IEnumerator EndInteractAnimation()
        {
            yield return new WaitForSeconds(3f);

            interactChecker.DisableIsChecking();
            interactChecker.StartCheckInteractTarget();

            anim.SetTrigger(hashEndInteract);
            canMoving = true;
        }

        #endregion
    }
}
