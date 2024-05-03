using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace lsy
{
    public class DialogueUIController : SceneUI, IPointerDownHandler
    {
        [SerializeField]
        private Text nameText;

        [SerializeField]
        private Text dialogueText;

        [SerializeField]
        private Button acceptButton;

        [SerializeField]
        private Button closeButton;


        public event Action onDialogueEnded;
        public event Action onDialougeClosed;
        public event Action onAcceptButtonClicked;

        private Coroutine typing;

        private List<string> currentDialogues;

        private bool isTyping;
        private bool isQuestDialogue;

        private int currentIndex;
        private int lastIndex;

        private readonly float typingSpeed = 0.05f;


        protected override void Awake()
        {
            base.Awake();
            Hide();

            acceptButton.onClick.AddListener(OnClickAcceptButton);
            closeButton.onClick.AddListener(() => Hide());

            acceptButton.GetComponentInChildren<Text>().text = StringManager.GetLocalizedUIText("Text_Accept");
            closeButton.GetComponentInChildren<Text>().text = StringManager.GetLocalizedUIText("Text_Close");
        }


        public override void Show(Action onShow = null)
        {
            base.Show(onShow);
            ShowNextDialogue();
        }


        public override void Hide(Action onHide = null)
        {
            base.Hide(onHide);
            acceptButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);

            onDialougeClosed?.Invoke();
            onDialougeClosed = null;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (currentDialogues == null)
                return;

            if (currentIndex == -1)
            {
                if (isQuestDialogue)
                {
                    return;
                }
                else
                {
                    Hide();
                    return;
                }                
            }

            ShowNextDialogue();
        }


        private void OnClickAcceptButton()
        {
            onAcceptButtonClicked?.Invoke();
            Hide();
        }


        public void SetAcceptButtonEvent(Action action)
        {
            onAcceptButtonClicked = action;
        }


        public void SetInitializeInfo(bool isQuest, string name, List<string> dialouges)
        {
            nameText.text = name;
            isQuestDialogue = isQuest;
            SetCurrentDialogues(dialouges);
        }


        private void SetCurrentDialogues(List<string> dialogues)
        {
            currentDialogues = dialogues;

            currentIndex = 0;
            lastIndex = currentDialogues.Count - 1;

            closeButton.gameObject.SetActive(false);
        }


        private void ShowNextDialogue()
        {
            if (isTyping)
            {
                StopCoroutine(typing);
                isTyping = false;
                dialogueText.text = currentDialogues[currentIndex];
                currentIndex++;

                if (currentIndex > lastIndex)
                {
                    AfterLastDialouge();
                }
            }
            else
            {
                if (typing != null)
                    StopCoroutine(typing);

                if (currentIndex != lastIndex)
                    typing = StartCoroutine(Typing(currentDialogues[currentIndex], () => currentIndex++));
                else
                    typing = StartCoroutine(Typing(currentDialogues[currentIndex], AfterLastDialouge));
            }
        }

        public IEnumerator Typing(string str, Action afterTyping = null)
        {
            isTyping = true;

            for (int i = 0; i < str.Length; i++)
            {
                dialogueText.text = str.Substring(0, i + 1);
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
            afterTyping?.Invoke();

            yield return null;
        }


        private void AfterLastDialouge()
        {
            currentIndex = -1;
            onDialogueEnded?.Invoke();
            onDialogueEnded = null;

            // 퀘스트가 있을때만
            if (isQuestDialogue)
            {
                acceptButton.gameObject.SetActive(true);
                closeButton.gameObject.SetActive(true);
            }
        }
    }
}
