using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueUIController : SceneUI, IPointerDownHandler
{
    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text dialogueText;

    [SerializeField]
    private Button closeButton;

    public event Action onEndDialogue;

    private Coroutine typing;
    private Coroutine showNextDialogue;

    private List<string> currentDialogues;

    private bool isTyping;

    private int currentIndex;
    private int lastIndex;
    private float typingSpeed = 0.15f;


    protected override void Awake()
    {
        base.Awake();
        Hide();
        closeButton.onClick.AddListener(() => Hide());
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
        ShowNextDialogue();
    }


    // ��ȭâ�� NPC �̸� ����
    public void SetNameText(string name)
    {
        nameText.text = name;
    }


    // ����ؾ��� ��� ����
    public void SetCurrentDialogues(List<string> dialogues)
    {
        currentDialogues = dialogues;

        currentIndex = 0;
        lastIndex = currentDialogues.Count - 1;

        closeButton.gameObject.SetActive(false);
    }


    // ���� ���� ���
    private void ShowNextDialogue()
    {
        if (currentDialogues == null || currentIndex == -1)
            return;

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
        onEndDialogue?.Invoke();

        closeButton.gameObject.SetActive(true);
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
