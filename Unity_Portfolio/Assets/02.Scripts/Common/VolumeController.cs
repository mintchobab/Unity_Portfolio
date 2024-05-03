using UnityEngine;
using UnityEngine.UI;

namespace lsy 
{
    public class VolumeController : MonoBehaviour
    {
        [SerializeField]
        private Button volumeButton;

        [SerializeField]
        private GameObject volumePanel;

        [SerializeField]
        private Slider bgmSlider;

        [SerializeField]
        private Slider sfxSlider;


        private bool isOpen = false;


        private void Awake()
        {
            volumeButton.onClick.AddListener(OnClickVolumeButton);

            bgmSlider.onValueChanged.AddListener(OnValueChangedBGM);
            sfxSlider.onValueChanged.AddListener(OnValueChangedSFX);
        }
        

        private void OnClickVolumeButton()
        {
            if (!isOpen)
            {
                isOpen = true;
                volumePanel.SetActive(true);
            }
            else
            {
                isOpen = false;
                volumePanel.SetActive(false);
            }
        }


        private void OnValueChangedBGM(float value)
        {
            Managers.Instance.SoundManager.ChangeVolumeBGM(value);
        }


        private void OnValueChangedSFX(float value)
        {
            Managers.Instance.SoundManager.ChangeVolumeSFX(value);
        }
    }
}
