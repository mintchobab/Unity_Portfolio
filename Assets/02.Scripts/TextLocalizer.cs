using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    [RequireComponent(typeof(Text))]
    public class TextLocalizer : MonoBehaviour
    {
        [SerializeField]
        private string textNameKey;

        private Text myText;


        private void Awake()
        {
            myText = GetComponent<Text>();
            myText.text = StringManager.GetLocalizedUIText(textNameKey);
        }
    }
}
