using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class PlayerInteractChecker : MonoBehaviour
    {
        [SerializeField]
        private TargetCircleController circleController;

        private List<IInteractable> interactList = new List<IInteractable>();

        private Collider coll;        
        private Coroutine findNearestInteractable;

        private bool isRunningFindInteractable;

        private float checkTime = 0.2f;


        private MainUIController inputController => Managers.Instance.UIManager.MainUIController;



        private void Awake()
        {
            coll = GetComponent<Collider>();
        }


        private void OnEnable()
        {
            isRunningFindInteractable = false;
        }


        private void OnDisable()
        {
            if (coll)
                coll.enabled = false;

            StopAllCoroutines();
            interactList.Clear();
        }


        private void OnTriggerEnter(Collider other)
        {
            EnterInteract(other);
        }


        private void OnTriggerExit(Collider other)
        {
            ExitInteract(other);
        }


        private void StopFindInteractable()
        {
            interactList.Clear();

            isRunningFindInteractable = false;

            if (findNearestInteractable != null)
                StopCoroutine(findNearestInteractable);
        }


        public void RestartFindInteractable()
        {
            StopFindInteractable();

            coll.enabled = false;
            coll.enabled = true;

            findNearestInteractable = StartCoroutine(FindNearestInteractable());
        }


        private void EnterInteract(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactList.Add(interactable);

                if (!isRunningFindInteractable)
                    findNearestInteractable = StartCoroutine(FindNearestInteractable());
            }
        }


        private void ExitInteract(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactList.Remove(interactable);

                if (interactList.Count == 0)
                {
                    isRunningFindInteractable = false;

                    if (findNearestInteractable != null)
                        StopCoroutine(findNearestInteractable);

                    inputController.SetBasicInteractButton();
                    circleController.HideCircle();
                }
            }
        }


        private IEnumerator FindNearestInteractable()
        {
            isRunningFindInteractable = true;

            IInteractable prevInteract = null;
            IInteractable currentInteract = null;

            while (true)
            {
                float minDist = 1000f;

                if (interactList.Count <= 0)
                {
                    StopFindInteractable();
                    yield break;
                }

                for (int i = 0; i < interactList.Count; i++)
                {
                    float dist = (transform.position - interactList[i].GetTransform().position).sqrMagnitude;

                    if (dist < minDist)
                    {
                        minDist = dist;
                        currentInteract = interactList[i];
                    }
                }

                if (prevInteract != currentInteract)
                {
                    inputController.SetInteractButton(currentInteract);

                    if (currentInteract is ITargetable)
                        circleController.ShowCircle(currentInteract.GetTransform(), Vector3.zero);
                    else
                        circleController.HideCircle();
                }

                prevInteract = currentInteract;

                yield return new WaitForSeconds(checkTime);
            }
        }
    }

}
