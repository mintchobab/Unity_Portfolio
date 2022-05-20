using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUIController : SceneUI
{
    [SerializeField]
    private Button dialogueButton;

    private event Action onInteract;


    private void Start()
    {
        dialogueButton.onClick.AddListener(OnClickInteractButton);
    }





    // 버튼 이미지 변경 + 버튼 이벤트 변경
    public void SetInteractButton(InteractBase interact)
    {
        // 어떤 버튼인지만 알아내면 되므로.. 스위치 필요없을지도..??

        switch (interact.InteractType)
        {
            case InteractType.Dialogue:
                SetDialogueButton(interact);
                break;

            case InteractType.Axe:
                break;
        }
    }


    private void OnClickInteractButton()
    {
        onInteract?.Invoke();
    }


    // 이벤트 변경
    // 버튼 활성화
    public void SetDialogueButton(InteractBase interact)
    {
        onInteract = interact.Interact;
        dialogueButton.gameObject.SetActive(true);
    }
}


