using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        private Coroutine showNextDialogue;

        private List<string> currentDialogues;

        private bool isTyping;
        private bool isQuestDialogue;

        private int currentIndex;
        private int lastIndex;
        private float typingSpeed = 0.15f;


        protected override void Awake()
        {
            base.Awake();
            Hide();

            acceptButton.onClick.AddListener(OnClickAcceptButton);
            closeButton.onClick.AddListener(() => Hide());

            acceptButton.GetComponentInChildren<Text>().text = StringManager.GetLocalizedUIText("Text_Accept");
            closeButton.GetComponentInChildren<Text>().text = StringManager.GetLocalizedUIText("Text_Close");
        }


        public override void Hide()
        {
            base.Hide();
            acceptButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);

            onDialougeClosed?.Invoke();
            onDialougeClosed = null;
        }


        // ĵ���� Ȱ��ȭ
        public override void Show()
        {
            base.Show();
            ShowNextDialogue();
        }



        // ��ȭâ or ȭ�� Ŭ���� ����
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


        // ���� ��ư Ŭ�� �̺�Ʈ
        private void OnClickAcceptButton()
        {
            onAcceptButtonClicked?.Invoke();
            Hide();
        }


        // ������ư�� �̺�Ʈ �߰�
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


        // ����ؾ��� ��� ����
        private void SetCurrentDialogues(List<string> dialogues)
        {
            currentDialogues = dialogues;

            currentIndex = 0;
            lastIndex = currentDialogues.Count - 1;

            closeButton.gameObject.SetActive(false);
        }



        // ���� ���� ���
        private void ShowNextDialogue()
        {
            // ��簡 ������� ��
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


        // ������ ��� ��� ���� ����
        private void AfterLastDialouge()
        {
            currentIndex = -1;
            onDialogueEnded?.Invoke();
            onDialogueEnded = null;

            // ����Ʈ�� ��������
            if (isQuestDialogue)
            {
                acceptButton.gameObject.SetActive(true);
                closeButton.gameObject.SetActive(true);
            }
        }



        // ��� Ÿ���� ȿ��
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

    }
}
