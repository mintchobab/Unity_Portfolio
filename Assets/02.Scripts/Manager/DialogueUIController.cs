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
    private List<string> currentDialogues;

    private float typingSpeed = 0.15f;
    private int currentIndex;
    private int lastIndex;


    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(() => Hide());
    }


    // ��ȭâ or ȭ�� Ŭ���� ����
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowNextSentence();
    }


    // ĵ���� Ȱ��ȭ
    public override void Show()
    {
        base.Show();
        ShowNextSentence();
    }


    // ��ȭâ�� NPC �̸� ����
    public void SetNameText(string name)
    {
        nameText.text = name;
    }


    // ��� Ÿ���� ȿ��
    public IEnumerator Typing(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            dialogueText.text = str.Substring(0, i + 1);
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return null;
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
    private void ShowNextSentence()
    {
        if (currentDialogues == null)
            return;

        if (currentIndex == -1)
            return;

        // ��ȭ ����
        if (currentIndex > lastIndex)
        {
            currentIndex = -1;
            onEndDialogue?.Invoke();

            closeButton.gameObject.SetActive(true);
            return;
        }

        // ��� ���� + Ÿ���� ȿ��
        if (typing != null)
            StopCoroutine(typing);

        typing = StartCoroutine(Typing(currentDialogues[currentIndex]));

        currentIndex++;
    }
}
