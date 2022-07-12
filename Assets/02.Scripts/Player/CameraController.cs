using System.Collections;
using UnityEngine;

namespace lsy
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField]
        private float distance;

        private Quaternion originRotation;

        private Vector3 prevPosition;
        private Quaternion prevRotation;
        private Coroutine moveCamera;

        private bool isFollowing = true;

        private InputUIController inputUIController => Managers.Instance.UIManager.InputUIController;



        public override void Init()
        {
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



        private void Zoom()
        {
            //distance -= Input.mouseScrollDelta.y;
            //distance = Mathf.Clamp(distance, 7f, 12f);
        }


        


        private void SetCurrentPositionAndRotation()
        {
            prevPosition = transform.position;
            prevRotation = transform.rotation;
        }


        // Ÿ���� �ٶ�
        public void LookTarget(Vector3 targetPosition, Quaternion lookTargetRotation)
        {
            SetCurrentPositionAndRotation();

            if (moveCamera != null)
                StopCoroutine(moveCamera);

            moveCamera = StartCoroutine(MoveCamera(targetPosition, lookTargetRotation));
        }


        // �÷��̾��� ������ �ٶ�
        public void LookPlayer()
        {
            SetCurrentPositionAndRotation();

            Vector3 targetPosition = PlayerController.Instance.PlayerLookPosition.position;
            Quaternion lookTargetRotation = PlayerController.Instance.PlayerLookPosition.rotation;

            if (moveCamera != null)
                StopCoroutine(moveCamera);

            moveCamera = StartCoroutine(MoveCamera(targetPosition, lookTargetRotation));
        }


        private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion lookTargetRotation)
        {
            isFollowing = false;

            // 60%���� �̸� �̵�
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.8f);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTargetRotation, 0.8f);

            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookTargetRotation, Time.deltaTime * 5f);
                yield return null;
            }
        }


        // ī�޶� ���� ��ġ�� ����
        public void RestoreCamera()
        {
            isFollowing = true;

            if (moveCamera != null)
                StopCoroutine(moveCamera);

            transform.position = prevPosition;
            transform.rotation = prevRotation;
        }


    }
}


