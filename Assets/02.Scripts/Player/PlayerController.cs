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

            // UI 변경
            //int maxHp = playerStat.FindStat(StatType.Hp).GetValue();        
            //hpController.ChangeHpUI(currentHp, maxHp);

            if (currentHp <= 0)
                Debug.LogWarning("사망");
        }




        #region Interact

        private InteractGauge currentGauge;
        private Coroutine interacting;
        private Coroutine endInteractAnimation;

        private bool isCompleted;


        // 상호작용 시작
        public void StartInteract(InteractData interactData, Transform interactObj)
        {
            isCompleted = false;
            canMoving = false;

            anim.SetTrigger(interactData.StartHash);
            equipController.MakeTool(interactData.InteractType, false);

            // UI 생성
            currentGauge = Managers.Instance.ResourceManager.Instantiate<InteractGauge>(ResourcePath.InteractGauge, billboardController.transform);
            billboardController.AddTarget(currentGauge.gameObject, interactObj, new Vector3(0f, 1f, 0f));

            currentGauge.Successed += InteractSuccessed;
            currentGauge.Failed += InteractFailed;

            // 작업별로 시간 다르게하기????????????????????????????
            currentGauge.StartProcess(0.8f, 5f, 8f);

            // 버튼 이미지 변경
            inputUIController.SetStopInteractButton(StopInteracting);

            if (endInteractAnimation != null)
                StopCoroutine(endInteractAnimation);

            // 코루틴 실행
            if (interacting != null)
                StopCoroutine(interacting);

            interacting = StartCoroutine(Interacting(interactData, interactObj));
        }



        // 상호작용 동작 (애니메이션 + 자리이동 + 오브젝트 바라보기)
        private IEnumerator Interacting(InteractData interactData, Transform target)
        {
            //  이동 & 회전
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
        


        // 상호작용 성공
        private void InteractSuccessed()
        {
            anim.SetTrigger(hashSuccessInteract);
            InteractCompleted();
        }


        // 상호작용 실패
        private void InteractFailed()
        {
            anim.SetTrigger(hashFailInteract);
            InteractCompleted();
        }


        // 상호작용 정지
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


        // 상호작용 종료
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

        
        // 상호작용 결과 애니메이션 종료

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
