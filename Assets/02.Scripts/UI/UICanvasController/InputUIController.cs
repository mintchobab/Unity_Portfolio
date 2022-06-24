using System;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class InputUIController : SceneUI
    {
        [SerializeField]
        private Button interactButton;

        [field: SerializeField]
        public Button InventoryButton { get; private set; }

        [SerializeField]
        private Button equipButton;

        private Sprite basicButtonSprite;
        private Sprite stopButtonSprite;
        private Sprite dialogueSprite;


        private event Action onInteracting;


        public Vector3 TouchRotateVector { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            
            LoadSprite();
            Show();
        }


        private void Start()
        {
            InventoryButton.onClick.AddListener(InventoryButtonClicked);
            equipButton.onClick.AddListener(EquipButtonClicked);
        }


        // 버튼 이미지 로드
        private void LoadSprite()
        {
            basicButtonSprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconBasic);
            stopButtonSprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconStop);
        }


        // 상호작용 기본 버튼으로 교체
        public void SetBasicInteractButton()
        {
            interactButton.image.sprite = basicButtonSprite;
            interactButton.onClick.RemoveAllListeners();
        }


        // 상호작용 정지 버튼으로 교체
        public void SetStopInteractButton(Action Stop)
        {
            interactButton.image.sprite = stopButtonSprite;

            onInteracting = null;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => Stop?.Invoke());
        }


        // 상호작용 실행 버튼으로 교체
        public void SetInteractButton(InteractBase interact)
        {
            interactButton.image.sprite = interact.LoadButtonImage();

            onInteracting = interact.Interact;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => onInteracting?.Invoke());
        }


        // 인벤토리 버튼 클릭
        private void InventoryButtonClicked()
        {
            Managers.Instance.UIManager.InventoryUIController.Show();
            Hide();
        }


        // 장비인벤토리 버튼 클릭
        private void EquipButtonClicked()
        {
            Managers.Instance.UIManager.EquipUIController.Show();
            Hide();
        }


        // 카메라 회전을 위한 벡터값 설정
        public void SetTouchVector(Vector3 vec)
        {
            TouchRotateVector = vec;
        }

    }
}
