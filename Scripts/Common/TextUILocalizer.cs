using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    [RequireComponent(typeof(Text))]
    public class TextUILocalizer : MonoBehaviour
    {
        [SerializeField]
        private string textNameKey;


        private void Awake()
        {
            Text myText = GetComponent<Text>();

            if (!textNameKey.Equals(string.Empty))
                myText.text = StringManager.GetLocalizedUIText(textNameKey);
        }
    }
}
