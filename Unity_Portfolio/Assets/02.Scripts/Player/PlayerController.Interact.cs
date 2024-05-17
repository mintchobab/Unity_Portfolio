using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lsy
{
    public partial class PlayerController
    {
        private WorldUIInteractGaugeCanvas gaugeCanvas;
        private Coroutine moveToInteractable;
        private Coroutine interactingCollection;
        private Coroutine endCollectingAnimation;

        private bool isCompleted;

        public void StartInteract(IInteractable interactable, Transform interactObj, Action startAction, Action endAction)
        {
            if (moveToInteractable != null)
                StopCoroutine(moveToInteractable);

            if (startAction != null)
                onStartInteract = () => startAction();

            if (endAction != null)
                onEndInteract = () => endAction();

            moveToInteractable = StartCoroutine(MoveToInteractable(interactable, interactObj));
        }


        private IEnumerator MoveToInteractable(IInteractable interactable, Transform interactObj)
        {
            yield return StartCoroutine(AutoMove(interactObj, interactable.GetInteractDistance()));

            onStartInteract?.Invoke();

            if (interactable is InteractCollection)
            {
                StartInteractCollection((InteractCollection)interactable, interactObj);
            }
            else if (interactable is InteractNpc)
            {
                StartInteractNpc((InteractNpc)interactable, interactObj);
            }
        }


        private void StartInteractNpc(InteractNpc interactNpc, Transform interactObj)
        {
            Vector3 targetPos = interactObj.position + (interactObj.forward * npcForwardDistance) + (Vector3.up * npcUpDistance);
            Quaternion targetRot = interactObj.rotation * Quaternion.Euler(0f, 180f, 0f);

            CameraController.Instance.LookNpc(targetPos, targetRot);
            dialogueController.onDialougeClosed += CameraController.Instance.RestoreCamera;
            dialogueController.onDialougeClosed += ShowModel;
            dialogueController.onDialougeClosed += interactNpc.OnDialougeClosed;


            // TODO : 조건이 중복되는거 같은데/......
            // => 네이밍이 이상하다
            if (interactNpc.IsExistQuest)
            {
                List<string> dialouges = null;
                bool isStartDialogue = false;

                if (questManager.CurrentQuest == null)
                {
                    dialouges = Tables.QuestTable[interactNpc.CurrentQuestId].StartDialogues.Split('/').ToList();

                    dialogueController.SetAcceptButtonEvent(() =>
                    {
                        questManager.TakeQuest(interactNpc.CurrentQuestId);
                        interactNpc.DestoryExclamationMark();
                    });

                    isStartDialogue = true;
                }
                else if (!questManager.CurrentQuest.IsCompleted)
                {
                    // false
                    dialouges = Tables.QuestTable[interactNpc.CurrentQuestId].ProgressingDialogues.Split('/').ToList();
                }
                else
                {
                    // false
                    dialouges = Tables.QuestTable[interactNpc.CurrentQuestId].EndDialogues.Split('/').ToList();

                    interactNpc.DestroyQuestionMark();
                    questManager.CompleteQuest();
                }

                dialogueController.SetInitializeInfo(isStartDialogue, interactNpc.NpcName, dialouges);
                dialogueController.Show(HideModel);

            }
            // 퀘스트가 없는 상황
            else
            {
                dialogueController.SetInitializeInfo(false, interactNpc.NpcName, interactNpc.GetDialogues());
                dialogueController.Show(HideModel);
            }
        }


        private void StartInteractCollection(InteractCollection interactCollection, Transform interactObj)
        {
            isCompleted = false;
            DisableCanMoving();

            anim.SetTrigger(interactCollection.MyCollectionData.AnimationHash);
            MakeTool(interactCollection.CollectionType);

            gaugeCanvas.gameObject.SetActive(true);
            gaugeCanvas.transform.position = interactObj.position + interactCollection.MyCollectionData.gaugePosition;
            gaugeCanvas.Successed += () => CollectingSuccessed(interactCollection, interactObj.position);
            gaugeCanvas.Failed += CollectingFailed;
            gaugeCanvas.StartProcess(0.8f, 5f, 8f);

            //inputUIController.SetStopInteractButton(StopCollecting);

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            if (interactingCollection != null)
                StopCoroutine(interactingCollection);

            Managers.Instance.SoundManager.Play(interactCollection.MyCollectionData.sfxName, SoundType.SFX_Long);

            interactingCollection = StartCoroutine(InteractingCollection(interactCollection.MyCollectionData, interactObj));
        }


        private IEnumerator InteractingCollection(CollectionData collectionData, Transform target)
        {
            float time = 0f;
            float timeToMove = 0.1f;

            Vector3 direction = (transform.position - target.position).normalized;
            Vector3 startPos = transform.position;
            Vector3 targetPos = target.position + direction * collectionData.InteractDistance;

            targetPos.y = transform.position.y;
            direction = Quaternion.Euler(0f, 180, 0f) * direction;
            direction.y = 0;

            while (time < 1)
            {
                time += Time.deltaTime / timeToMove;
                transform.position = Vector3.Lerp(startPos, targetPos, time);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), time);
                yield return null;
            }
        }


        private void CollectingSuccessed(InteractCollection interactCollection, Vector3 targetPosition)
        {
            anim.SetTrigger(hashSuccessInteract);

            Managers.Instance.InventoryManager.AddItem(interactCollection.ItemId, 1);
            Managers.Instance.UIManager.SystemUIController.GetItem(interactCollection, targetPosition);

            Managers.Instance.SoundManager.Play("Success", SoundType.SFX);

            CompletedCollecting();
        }


        private void CollectingFailed()
        {
            anim.SetTrigger(hashFailInteract);
            Managers.Instance.SoundManager.Play("Fail", SoundType.SFX);

            CompletedCollecting();
        }


        public void StopCollecting()
        {
            if (isCompleted)
                return;

            if (interactingCollection != null)
                StopCoroutine(interactingCollection);

            EnableCanMoving();

            anim.SetTrigger(hashEndInteract);

            DestoryCurrentTool();
            interactChecker.RestartFindInteractable();

            gaugeCanvas.gameObject.SetActive(false);
        }


        private void CompletedCollecting()
        {
            isCompleted = true;

            gaugeCanvas.gameObject.SetActive(false);

            onEndInteract?.Invoke();
            onEndInteract = null;

            Managers.Instance.SoundManager.Stop(SoundType.SFX_Long);

            if (endCollectingAnimation != null)
                StopCoroutine(endCollectingAnimation);

            endCollectingAnimation = StartCoroutine(EndCollectingAnimation());
        }


        private IEnumerator EndCollectingAnimation()
        {
            yield return new WaitForSeconds(afterInteractDelay);

            DestoryCurrentTool();

            interactChecker.RestartFindInteractable();

            anim.SetTrigger(hashEndInteract);
            EnableCanMoving();
        }
    }
}
