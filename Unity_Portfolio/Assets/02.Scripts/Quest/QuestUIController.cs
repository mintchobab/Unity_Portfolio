using UnityEngine;

namespace lsy 
{
    public class QuestUIController : SceneUI
    {
        [SerializeField]
        private QuestUIInfoPanel questInfoPanel;

        [SerializeField]
        private QuestUIProgressBar questProgressBar;


        protected override void Awake()
        {
            base.Awake();

            questInfoPanel.Initailize();
            questProgressBar.Initialize();
        }
    }
}
