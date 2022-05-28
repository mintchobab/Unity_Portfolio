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


        // 상호작용 버튼 초기화 (일단 정지로)
        public void ResetInteractButton()
        {
            onInteract = null;
            interactButton.image.sprite = defaultSprite;
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
