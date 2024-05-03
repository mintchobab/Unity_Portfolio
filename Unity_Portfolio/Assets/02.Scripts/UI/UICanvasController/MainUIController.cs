using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class MainUIController : SceneUI
    {
        [SerializeField]
        private Button interactButton;

        [field: SerializeField]
        public Button InventoryButton { get; private set; }

        [SerializeField]
        private Button equipButton;

        [SerializeField]
        private GameObject basicButtons;

        [Header("스킬 관련")]
        [SerializeField]
        private GameObject combatButtonParent;

        [SerializeField]
        private AttackButton attackButton;

        [field: SerializeField]
        public SkillButton[] SkillButtons { get; private set; }

        private event Action onInteracting;

        private Sprite basicButtonSprite;
        private Sprite stopButtonSprite;

        public Vector3 TouchRotateVector { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            LoadSprite();
            Show();
        }


        private void Start()
        {
            InventoryButton.onClick.AddListener(OnClickedInventoryButton);
            equipButton.onClick.AddListener(OnClickedEquipButton);
        }


        private void LoadSprite()
        {
            basicButtonSprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconBasic);
            stopButtonSprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconStop);
        }


        public void SetBasicInteractButton()
        {
            interactButton.image.sprite = basicButtonSprite;
            interactButton.onClick.RemoveAllListeners();
        }


        public void SetStopInteractButton(Action stopInteract)
        {
            interactButton.image.sprite = stopButtonSprite;

            onInteracting = null;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => stopInteract?.Invoke());
        }


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
            interactButton.onClick.AddListener(() => action?.Invoke());
        }


        public void SetTouchVector(Vector3 vec)
        {
            TouchRotateVector = vec;
        }


        public void ActivateCombatButton()
        {
            combatButtonParent.SetActive(true);
            basicButtons.SetActive(false);
        }


        public void ChangeSkillButtons(PlayerSkill normalAttack, PlayerSkill[] playerSkills, Action<PlayerSkill, CombatButton> onClickButton)
        {
            attackButton.SetButton(normalAttack.SkillIcon);
            attackButton.SkillButton.onClick.AddListener(() => onClickButton(normalAttack, attackButton));

            for (int i = 0; i < playerSkills.Length; i++)
            {
                int j = i;

                SkillButtons[j].SetButton(playerSkills[j].SkillIcon, playerSkills[j].GetSkillName());
                SkillButtons[j].SkillButton.onClick.AddListener(() => onClickButton(playerSkills[j], SkillButtons[j]));
            }
        }


        public void SizeUpSkillButtons()
        {
            attackButton.SizeDown();

            foreach (var button in SkillButtons)
            {
                button.MoveAndSizeUp();
            }
        }


        public void SizeDownSkillButtons()
        {
            attackButton.SizeUp();

            foreach (var button in SkillButtons)
            {
                button.MoveAndSizeDown();
            }
        }

        private void OnClickedInventoryButton()
        {
            Managers.Instance.UIManager.InventoryUIController.Show();
            Hide();
        }


        private void OnClickedEquipButton()
        {
            Managers.Instance.UIManager.EquipUIController.Show();
            Hide();
        }

    }
}
