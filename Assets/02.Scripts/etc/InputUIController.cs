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


    private void OnClickQuestButton()
    {
        Managers.Instance.UIManager.QuestUIController.Show();
    }

    private void OnClickInventoryButton()
    {
        Managers.Instance.UIManager.InventoryUIController.Show();
    }


    // 이벤트 변경
    // 버튼 활성화
    public void SetDialogueButton(InteractBase interact)
    {
        onInteract = interact.Interact;
        dialogueButton.gameObject.SetActive(true);
    }
}


