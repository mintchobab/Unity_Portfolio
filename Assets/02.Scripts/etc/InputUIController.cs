using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class InputUIController : SceneUI
    {
        [SerializeField]
        private Button interactButton;

        [SerializeField]
        private Button questButton;

        [SerializeField]
        private Button inventoryButton;


        [SerializeField]
        private Sprite defaultSprite;

        [SerializeField]
        private Sprite dialogueSprite;



        private event Action onInteract;


        protected override void Awake()
        {
            base.Awake();
            Show();
        }


        private void Start()
        {
            interactButton.onClick.AddListener(() => onInteract?.Invoke());
            questButton.onClick.AddListener(OnClickQuestButton);
            inventoryButton.onClick.AddListener(OnClickInventoryButton);
        }


        // ��ȣ�ۿ� ��ư �ʱ�ȭ (�ϴ� ������)
        public void ResetInteractButton()
        {
            onInteract = null;
            interactButton.image.sprite = defaultSprite;
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

                case InteractType.WoodChop:
                    break;
            }
        }

        private void OnClickQuestButton()
        {
            Managers.Instance.UIManager.QuestUIController.Show();
        }

        private void OnClickInventoryButton()
        {
            Managers.Instance.UIManager.InventoryUIController.Show();
        }


        public void SetDialogueButton(InteractBase interact)
        {
            onInteract = interact.Interact;
            interactButton.image.sprite = dialogueSprite;
        }
    }
}
