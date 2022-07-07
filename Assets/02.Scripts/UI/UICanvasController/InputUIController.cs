using System;
using System.Collections.Generic;
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

        //[SerializeField]
        //private GameObject

        [SerializeField]
        private CombatButtonController combatButtonController;

        private Sprite basicButtonSprite;
        private Sprite stopButtonSprite;


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
        public void SetStopInteractButton(Action stopInteract)
        {
            interactButton.image.sprite = stopButtonSprite;

            onInteracting = null;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => stopInteract?.Invoke());
        }


        // 상호작용 실행 버튼으로 교체
        public void SetInteractButton(IInteractable interactable)
        {
            interactButton.image.sprite = interactable.LoadButtonImage();

            onInteracting = interactable.Interact;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => onInteracting?.Invoke());
        }


        public void SetCombatReadyButton(Action action)
        {
            interactButton.image.sprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconCombat);

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(action.Invoke);
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


        public void ChangeSkill(List<SkillData> skillDatas)
        {
            if (combatButtonController == null)
                Debug.LogWarning("여기");

            combatButtonController.ChangeSkillButton(skillDatas);
        }

    }
}
