using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class StartSceneController : MonoBehaviour
    {
        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Button languageButton;

        [SerializeField]
        private Button quitButton;

        [Header("Popup")]
        [SerializeField]
        private Text popupTitle;

        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private GameObject languegePopup;

        [SerializeField]
        private Toggle koreanToggle;

        [SerializeField]
        private Toggle englishToggle;



        private void Start()
        {
            startButton.onClick.AddListener(OnClickStartButton);
            languageButton.onClick.AddListener(OnClickLanguageButton);
            quitButton.onClick.AddListener(OnClickQuitButton);

            exitButton.onClick.AddListener(() => languegePopup.gameObject.SetActive(false));
            koreanToggle.onValueChanged.AddListener(OnValueChangedKoreanToggle);
            englishToggle.onValueChanged.AddListener(OnValueChangedEnglishToggle);

            PlayerPrefs.SetString("Language", "Korean");
        }


        private void OnClickStartButton()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }


        private void OnClickLanguageButton()
        {
            languegePopup.gameObject.SetActive(true);
        }


        private void OnClickQuitButton()
        {
            Application.Quit();
        }


        public void OnValueChangedKoreanToggle(bool value)
        {
            if (value)
            {
                popupTitle.text = "언어 선택";
                startButton.GetComponentInChildren<Text>().text = "시작";
                languageButton.GetComponentInChildren<Text>().text = "언어 설정";
                quitButton.GetComponentInChildren<Text>().text = "종료";
                PlayerPrefs.SetString("Language", "Korean");
            }
        }


        public void OnValueChangedEnglishToggle(bool value)
        {
            if (value)
            {
                popupTitle.text = "Language";
                startButton.GetComponentInChildren<Text>().text = "Start";
                languageButton.GetComponentInChildren<Text>().text = "Languege";
                quitButton.GetComponentInChildren<Text>().text = "Quit";
                PlayerPrefs.SetString("Language", "English");
            }           
        }

    }
}
