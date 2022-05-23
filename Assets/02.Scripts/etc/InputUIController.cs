using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUIController : SceneUI
{
    [SerializeField]
    private Button dialogueButton;

    [SerializeField]
    private Button questButton;

    [SerializeField]
    private Button inventoryButton;

    private event Action onInteract;


    protected override void Awake()
    {
        base.Awake();
        Show();
    }


    private void Start()
    {
        dialogueButton.onClick.AddListener(OnClickInteractButton);
        questButton.onClick.AddListener(OnClickQuestButton);
        inventoryButton.onClick.AddListener(OnClickInventoryButton);
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


    private void OnClickQuestButton()
    {
        Managers.Instance.UIManager.QuestUIController.Show();
    }

    private void OnClickInventoryButton()
    {
        Managers.Instance.UIManager.InventoryUIController.Show();
    }


    // �̺�Ʈ ����
    // ��ư Ȱ��ȭ
    public void SetDialogueButton(InteractBase interact)
    {
        onInteract = interact.Interact;
        dialogueButton.gameObject.SetActive(true);
    }
}


