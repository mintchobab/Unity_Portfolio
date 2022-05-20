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





    // ��ư �̹��� ���� + ��ư �̺�Ʈ ����
    public void SetInteractButton(InteractBase interact)
    {
        // � ��ư������ �˾Ƴ��� �ǹǷ�.. ����ġ �ʿ��������..??

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


    // �̺�Ʈ ����
    // ��ư Ȱ��ȭ
    public void SetDialogueButton(InteractBase interact)
    {
        onInteract = interact.Interact;
        dialogueButton.gameObject.SetActive(true);
    }
}


