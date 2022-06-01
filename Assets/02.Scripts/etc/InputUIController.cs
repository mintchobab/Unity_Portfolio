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



        // 없애기......
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


        // 퀘스트 버튼 클릭
        private void OnClickQuestButton()
        {
            Managers.Instance.UIManager.QuestUIController.Show();
            Hide();
        }


        // 인벤토리 버튼 클릭
        private void OnClickInventoryButton()
        {
            Managers.Instance.UIManager.InventoryUIController.Show();
            Hide();
        }


        // 장비인벤토리 버튼 클릭
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
