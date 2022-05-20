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


    // 대화창 or 화면 클릭시 실행
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowNextSentence();
    }


    // 캔버스 활성화
    public override void Show()
    {
        base.Show();
        ShowNextSentence();
    }


    // 대화창의 NPC 이름 변경
    public void SetNameText(string name)
    {
        nameText.text = name;
    }


    // 대사 타이핑 효과
    public IEnumerator Typing(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            dialogueText.text = str.Substring(0, i + 1);
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return null;
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
    private void ShowNextSentence()
    {
        if (currentDialogues == null)
            return;

        if (currentIndex == -1)
            return;

        // 대화 종료
        if (currentIndex > lastIndex)
        {
            currentIndex = -1;
            onEndDialogue?.Invoke();

            closeButton.gameObject.SetActive(true);
            return;
        }

        // 대사 변경 + 타이핑 효과
        if (typing != null)
            StopCoroutine(typing);

        typing = StartCoroutine(Typing(currentDialogues[currentIndex]));

        currentIndex++;
    }
}
