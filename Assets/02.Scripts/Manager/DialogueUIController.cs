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


    // 캔버스 활성화
    public override void Show()
    {
        base.Show();

        ShowNextDialogue();
    }


    // 대화창 or 화면 클릭시 실행
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowNextDialogue();
    }


    // 대화창의 NPC 이름 변경
    public void SetNameText(string name)
    {
        nameText.text = name;
    }


    // 출력해야할 대사 설정
    public void SetCurrentDialogues(List<string> dialogues)
    {
        currentDialogues = dialogues;

        currentIndex = 0;
        lastIndex = currentDialogues.Count - 1;

        closeButton.gameObject.SetActive(false);
    }


    // 다음 문장 출력
    private void ShowNextDialogue()
    {
        if (currentDialogues == null || currentIndex == -1)
            return;

        // 대사가 출력중일 때
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


    // 마지막 대사 출력 이후 실행
    private void AfterLastDialouge()
    {
        currentIndex = -1;
        onEndDialogue?.Invoke();

        closeButton.gameObject.SetActive(true);
    }



    // 대사 타이핑 효과
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
