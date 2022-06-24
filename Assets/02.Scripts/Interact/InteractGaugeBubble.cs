using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class InteractGaugeBubble : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Text resultText;


        public void ChangeBubbleColor(Color color)
        {
            image.color = color;
        }

        public void ChangeResultText(string str)
        {
            resultText.text = str;
        }
    }
}
