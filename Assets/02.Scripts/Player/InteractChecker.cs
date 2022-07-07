using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class InteractChecker : MonoBehaviour
    {
        private List<IInteractable> interacts = new List<IInteractable>();
        private List<MonsterBT> monsterList = new List<MonsterBT>();


        private TargetCircleController circleController;
        private MonsterChecker monsterChecker;
        private Collider coll;
        
        private Coroutine checkInteractable;
        private Coroutine checkMonster;

        private bool isProcessing;
        private bool isCheckingMonster;


        private InputUIController inputController => Managers.Instance.UIManager.InputUIController;



        private void Awake()
        {
            coll = GetComponent<Collider>();
            circleController = GetComponent<TargetCircleController>();
        }


        private void OnEnable()
        {
            isProcessing = false;

            coll.enabled = false;
            coll.enabled = true;
        }


        private void OnDisable()
        {
            interacts.Clear();
            monsterChecker.enabled = true;
        }


        public void RestartChecking()
        {
            isProcessing = false;
            StartCheckInteractable();
        }



        public void ChangeFindInteract()
        {
            isCheckingMonster = false;
        }

        public void ChangeFindMonster()
        {
            isCheckingMonster = true;
        }




        private void OnTriggerEnter(Collider other)
        {
            if (!isCheckingMonster)
            {
                EnterInteract(other);
            }
            else
            {
                EnterMonster(other);
            }
        }



        private void OnTriggerExit(Collider other)
        {
            if (!isCheckingMonster)
            {
                ExitInteract(other);
            }
            else
            {
                ExitMonster(other);
            }
        }




        private void EnterInteract(Collider other)
        {
            // ��ȣ�ۿ� ���
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interacts.Add(interactable);
                StartCheckInteractable();
            }
        }


        private void EnterMonster(Collider other)
        {
            MonsterBT monster = other.GetComponent<MonsterBT>();
            if (monster != null)
            {
                monsterList.Add(monster);


            }
        }


        private void ExitInteract(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interacts.Remove(interactable);

                if (interacts.Count == 0)
                {
                    isProcessing = false;

                    if (checkInteractable != null)
                        StopCoroutine(checkInteractable);

                    // �⺻ ��ư���� ����
                    inputController.SetBasicInteractButton();
                    circleController.HideCircle();
                }
            }
        }

        private void ExitMonster(Collider other)
        {

        }






        // ��ȣ�ۿ� ��� ã��
        private void StartCheckInteractable()
        {
            if (isProcessing)
                return;

            isProcessing = true;

            if (checkInteractable != null)
                StopCoroutine(checkInteractable);

            checkInteractable = StartCoroutine(CheckInteractable());
        }


        // ���� ����� Ÿ���� ���ϱ�
        private IEnumerator CheckInteractable()
        {
            IInteractable prevInteract = null;
            IInteractable currentInteract = null;

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
                    float dist = (transform.position - interacts[i].GetTransform().position).sqrMagnitude;

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

                    // ���� �Ǻ��� ��ġ�� �ٲ�� �Ѵٸ� ���⼭ �ϱ�
                    if (currentInteract is ITargetable)
                        circleController.ShowCircle(currentInteract.GetTransform(), Vector3.zero);
                    else
                        circleController.HideCircle();
                }

                prevInteract = currentInteract;

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}

