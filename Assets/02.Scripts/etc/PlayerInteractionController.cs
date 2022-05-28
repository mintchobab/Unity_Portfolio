using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class PlayerInteractionController : MonoBehaviour
    {
        private InputUIController inputController;

        private Coroutine checkInteractTarget;

        private List<InteractBase> interacts = new List<InteractBase>();
        // private InteractBase currentInteract;


        private void Start()
        {
            inputController = Managers.Instance.UIManager.InputController;
        }


        // 겹치는 경우도 있겠다.....
        private void OnTriggerEnter(Collider other)
        {
            InteractBase interact = other.GetComponent<InteractBase>();

            if (interact != null)
            {
                interacts.Add(interact);

                if (checkInteractTarget != null)
                    StopCoroutine(checkInteractTarget);

                checkInteractTarget = StartCoroutine(CheckInteractTarget());
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
                    if (checkInteractTarget != null)
                        StopCoroutine(checkInteractTarget);

                    inputController.ResetInteractButton();
                }
            }
        }


        // 가장 가까운 타겟을 정하기
        private IEnumerator CheckInteractTarget()
        {
            InteractBase prevInteract = null;
            InteractBase currentInteract = null;
            float minDist = 1000f;

            while (true)
            {
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

                if (prevInteract != currentInteract)
                    inputController.SetInteractButton(currentInteract);

                prevInteract = currentInteract;

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}

