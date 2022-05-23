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
    private Button acceptButton;

    [SerializeField]
    private Button closeButton;


    public event Action onEndDialogue;
    public event Action onAcceptEvent;


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

        acceptButton.onClick.AddListener(OnClickAcceptButton);
        closeButton.onClick.AddListener(() => Hide());
    }


    public override void Hide()
    {
        base.Hide();
        acceptButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
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


    // ���� ��ư Ŭ�� �̺�Ʈ
    private void OnClickAcceptButton()
    {
        onAcceptEvent?.Invoke();
        onAcceptEvent = null;
        Hide();
    }


    // ������ư�� �̺�Ʈ �߰�
    public void SetAcceptButtonEvent(Action action)
    {
        onAcceptEvent += action;
        acceptButton.GetComponentInChildren<Text>().text = "����Ʈ ����";
    }


    // ��ȭâ�� NPC �̸� ����
    public void SetNameText(string name)
    {
        nameText.text = name;
    }


    // ��ȭ �������� �̺�Ʈ �߰�
    public void SetEndEvent(Action action)
    {
        onEndDialogue += action;
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
        onEndDialogue = null;

        // ����Ʈ�� ��������
        // �ٵ� �̰� �Ǵ��ϴ°� ���⼭ �ؾߵɱ�....?????
        acceptButton.gameObject.SetActive(true);
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
