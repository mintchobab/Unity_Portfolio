using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class PlayerInteractChecker : MonoBehaviour
    {
        [SerializeField]
        private List<InteractBase> interacts = new List<InteractBase>();

        private Coroutine checkInteractTarget;
        private InputUIController inputController;
        private InteractCircle interactCircle;
        private BillboardUIController billboardUIController => Managers.Instance.UIManager.BillboardUIController;


        private bool isChecking;


        private void Start()
        {
            inputController = Managers.Instance.UIManager.InputUIController;
            interactCircle = Managers.Instance.ResourceManager.Instantiate<InteractCircle>(ResourcePath.InteractCircle, billboardUIController.transform);
            interactCircle.Hide();
        }


        private void OnTriggerEnter(Collider other)
        {
            InteractBase interact = other.GetComponent<InteractBase>();

            if (interact != null)
            {
                interacts.Add(interact);

                StartCheckInteractTarget();
            }
        }


        private void OnTriggerExit(Collider other)
        {
            InteractBase interact = other.GetComponent<InteractBase>();

            if (interact != null)
            {
                interacts.Remove(interact);

                if (interacts.Count == 0)
                {
                    isChecking = false;

                    if (checkInteractTarget != null)
                        StopCoroutine(checkInteractTarget);

                    // 기본 버튼으로 변경
                    inputController.SetBasicInteractButton();

                    interactCircle.Hide();
                }
            }
        }

        public void DisableIsChecking()
        {
            isChecking = false;
        }


        // 상호작용 대상 찾기
        public void StartCheckInteractTarget()
        {
            if (isChecking)
                return;

            isChecking = true;

            if (checkInteractTarget != null)
                StopCoroutine(checkInteractTarget);
            
            checkInteractTarget = StartCoroutine(CheckInteractTarget());
        }


        // 가장 가까운 타겟을 정하기
        private IEnumerator CheckInteractTarget()
        {
            InteractBase prevInteract = null;
            InteractBase currentInteract = null;

            prevInteract = null;
            currentInteract = null;

            while (true)
            {
                float minDist = 1000f;

                if (interacts.Count <= 0)
                {
                    yield break;
                }

                for (int i = 0; i < interacts.Count; i++)
                {
                    float dist = (transform.position - interacts[i].transform.position).sqrMagnitude;

                    if (dist < minDist)
                    {
                        minDist = dist;
                        currentInteract = interacts[i];
                    }
                }

                // Interact 타겟이 바꼈을 때
                if (prevInteract != currentInteract)
                {
                    inputController.SetInteractButton(currentInteract);

                    // 서클의 위치 변경 해야됨~```````````````````
                    interactCircle.Show(currentInteract.transform);                    
                }

                prevInteract = currentInteract;

                yield return new WaitForSeconds(0.2f);
            }            
        }
    }
}

