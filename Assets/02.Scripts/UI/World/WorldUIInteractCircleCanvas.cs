using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class WorldUIInteractCircleCanvas : MonoBehaviour
    {
        private Canvas canvas;
        private Transform originParent;
        private RectTransform rectTransform;
        private Animator anim;

        private Coroutine followTarget;


        public void Initialize(Transform parent)
        {
            canvas = GetComponent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            anim = GetComponentInChildren<Animator>();

            canvas.worldCamera = Camera.main;            
            originParent = parent;

            Hide();
        }


        // ��ġ ����
        public void Show(Transform target, Vector3 addedPosition)
        {
            rectTransform.SetParent(null);
            canvas.enabled = true;
            anim.enabled = true;

            if (followTarget != null)
                StopCoroutine(followTarget);

            followTarget = StartCoroutine(FollowTarget(target, addedPosition));
        }


        // �θ�� ���� ��ġ��
        public void Hide()
        {
            // �ִϸ��̼� ��� ����Ǵ��� Ȯ���ϱ�.��.... �Ƹ��� �ɰ�?? => ����

            canvas.enabled = false;
            anim.enabled = false;
            rectTransform.SetParent(originParent);

            if (followTarget != null)
                StopCoroutine(followTarget);
        }


        private IEnumerator FollowTarget(Transform target, Vector3 addedPosition)
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                transform.position = target.position + addedPosition;
            }            
        }

    }
}
