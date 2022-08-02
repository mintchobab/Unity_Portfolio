using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace lsy
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField]
        private float distance;

        [SerializeField]
        private AnimationCurve shakeCurve;

        private Quaternion originRotation;

        private Vector3 prevPosition;
        private Quaternion prevRotation;

        private Coroutine moveCamera;
        private Coroutine shaking;
        private Camera cam;

        private bool isFollowing = true;

        private MainUIController inputUIController => Managers.Instance.UIManager.MainUIController;



        public override void Init()
        {
            cam = Camera.main;
            originRotation = transform.rotation;
        }


        void LateUpdate()
        {
            if (isFollowing)
            {
                Vector3 direction = originRotation * Vector3.forward;
                direction = Quaternion.Euler(inputUIController.TouchRotateVector) * direction;
                direction.Normalize();

                transform.position = (PlayerController.Instance.transform.position + new Vector3(0f, 0.8f, 0f)) - direction * distance;
                transform.rotation = Quaternion.LookRotation(direction);
            }            
        }


        public void StartFolloing()
        {
            isFollowing = true;
        }

        public void StopFolloing()
        {
            isFollowing = false;
        }


        // position, rotation 값 저장
        private void SetCurrentPositionAndRotation()
        {
            prevPosition = transform.position;
            prevRotation = transform.rotation;
        }


        // 타겟을 바라봄
        public void LookTarget(Vector3 targetPosition, Quaternion lookTargetRotation)
        {
            SetCurrentPositionAndRotation();

            if (moveCamera != null)
                StopCoroutine(moveCamera);

            moveCamera = StartCoroutine(MoveCamera(targetPosition, lookTargetRotation));
        }


        // 플레이어의 정면을 바라봄
        public void LookPlayer()
        {
            SetCurrentPositionAndRotation();

            Vector3 targetPosition = PlayerController.Instance.PlayerLookPosition.position;
            Quaternion lookTargetRotation = PlayerController.Instance.PlayerLookPosition.rotation;

            if (moveCamera != null)
                StopCoroutine(moveCamera);

            moveCamera = StartCoroutine(MoveCamera(targetPosition, lookTargetRotation));
        }


        // 카메라가 특정 위치로 이동 (ex. npc 대화)
        private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion lookTargetRotation)
        {
            StopFolloing();

            // 60%정도 미리 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.8f);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTargetRotation, 0.8f);

            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookTargetRotation, Time.deltaTime * 5f);
                yield return null;
            }
        }


        // 카메라를 원래 위치로 복귀
        public void RestoreCamera()
        {
            StartFolloing();

            if (moveCamera != null)
                StopCoroutine(moveCamera);

            transform.position = prevPosition;
            transform.rotation = prevRotation;
        }


        // 카메라 흔들기 시작 (중복 적용 막기 위해)
        public void StartShaking(float time = 0.1f, float intensity = 1f)
        {
            if (shaking != null)
                StopCoroutine(shaking);

            shaking = StartCoroutine(Shaking(time, intensity));
        }


        // 카메라 흔들기
        private IEnumerator Shaking(float time, float intensity)
        {
            float t = 0f;

            while (t < 1)
            {
                t += Time.deltaTime / time;

                float value = shakeCurve.Evaluate(t) * intensity;
                cam.transform.localPosition = new Vector3(value, 0f, 0f);
                yield return null;
            }

            cam.transform.localPosition = Vector3.zero;
            yield return null;
        }

    }
}


