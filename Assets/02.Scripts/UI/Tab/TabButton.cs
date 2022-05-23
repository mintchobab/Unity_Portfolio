using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject tabPage;

        private Image image;
        private Button button;

        private TabController tabController;


        public void Initialized(TabController tabController)
        {
            this.tabController = tabController;

            image = GetComponent<Image>();
            button = GetComponent<Button>();

            button.onClick.AddListener(OnClickButton);

            image.sprite = tabController.DefaultImage;
            tabPage.gameObject.SetActive(false);
        }


        private void OnClickButton()
        {
            tabController.ClickedTabButton(this);
            Selected();
        }


        public void Selected()
        {
            image.sprite = tabController.SelectedImage;
            tabPage.gameObject.SetActive(true);
        }

        public void UnSelected()
        {
            image.sprite = tabController.DefaultImage;
            tabPage.gameObject.SetActive(false);
        }
    }

}

