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

                    // �⺻ ��ư���� ����
                    inputController.SetBasicInteractButton();

                    interactCircle.Hide();
                }
            }
        }

        public void DisableIsChecking()
        {
            isChecking = false;
        }


        // ��ȣ�ۿ� ��� ã��
        public void StartCheckInteractTarget()
        {
            if (isChecking)
                return;

            isChecking = true;

            if (checkInteractTarget != null)
                StopCoroutine(checkInteractTarget);
            
            checkInteractTarget = StartCoroutine(CheckInteractTarget());
        }


        // ���� ����� Ÿ���� ���ϱ�
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

                // Interact Ÿ���� �ٲ��� ��
                if (prevInteract != currentInteract)
                {
                    inputController.SetInteractButton(currentInteract);

                    // ��Ŭ�� ��ġ ���� �ؾߵ�~```````````````````
                    interactCircle.Show(currentInteract.transform);                    
                }

                prevInteract = currentInteract;

                yield return new WaitForSeconds(0.2f);
            }            
        }
    }
}

