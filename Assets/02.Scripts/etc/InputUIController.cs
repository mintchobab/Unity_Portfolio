using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace lsy
{
    public class InputUIController : SceneUI, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Button interactButton;

        [SerializeField]
        private Button questButton;

        [SerializeField]
        private Button inventoryButton;

        [SerializeField]
        private Button equipButton;



        // ���ֱ�......
        [SerializeField]
        private Sprite defaultSprite;

        [SerializeField]
        private Sprite dialogueSprite;



        private event Action onInteract;
        private Coroutine rotateCamera;



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
            equipButton.onClick.AddListener(OnClickEquipButton);
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            if (rotateCamera != null)
                StopCoroutine(rotateCamera);

            rotateCamera = StartCoroutine(RotateCamera());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (rotateCamera != null)
                StopCoroutine(rotateCamera);
        }



        private Vector3 prevPos;
        private float mouseDragDist;
        public Vector3 TestRot { get; private set; }


        private IEnumerator RotateCamera()
        {
            prevPos = Input.mousePosition;

            while (true)
            {
                Vector3 newPos = Input.mousePosition;
                Vector3 dist = newPos - prevPos;

                mouseDragDist += dist.x;

                TestRot = new Vector3(0f, mouseDragDist, 0f);
                prevPos = newPos;
                yield return null;
            }
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


        // ����Ʈ ��ư Ŭ��
        private void OnClickQuestButton()
        {
            Managers.Instance.UIManager.QuestUIController.Show();
            Hide();
        }


        // �κ��丮 ��ư Ŭ��
        private void OnClickInventoryButton()
        {
            Managers.Instance.UIManager.InventoryUIController.Show();
            Hide();
        }


        // ����κ��丮 ��ư Ŭ��
        private void OnClickEquipButton()
        {
            Managers.Instance.UIManager.EquipUIController.Show();
            Hide();
        }


        public void SetDialogueButton(InteractBase interact)
        {
            onInteract = interact.Interact;
            interactButton.image.sprite = dialogueSprite;
        }


    }
}
