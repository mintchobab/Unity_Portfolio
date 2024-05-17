using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class WorldUIName : MonoBehaviour, IWorldUI
    {
        [SerializeField]
        private NameType nameType;

        [SerializeField]
        private string nameKey;

        [field: SerializeField]
        public Vector3 LocalPosition { get; private set; }


        private readonly string npcNameColorString = "#316DDB";
        private readonly string collectionNameColorString = "#D0EC33";

        public WorldUINameCanvas NameCanvas { get; private set; }


        private void Awake()
        {
            NameCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUINameCanvas>(ResourcePath.WorldNameCanvas);

            string name = string.Empty;
            string colorString = string.Empty;            

            switch (nameType)
            {
                case NameType.Player:
                    break;

                case NameType.Npc:
                    name = StringManager.Get(nameKey);
                    colorString = npcNameColorString;
                    break;

                case NameType.Monster:
                    break;

                case NameType.Collection:
                    name = StringManager.Get(nameKey);
                    colorString = collectionNameColorString;
                    break;
            }

            ColorUtility.TryParseHtmlString(colorString, out Color color);

            NameCanvas.SetName(name);
            NameCanvas.SetNameColor(color);
            NameCanvas.SetParent(transform);
            NameCanvas.SetLocalPosition(LocalPosition);
            NameCanvas.Hide();
        }


        public void Show()
        {
            NameCanvas.Show();
        }


        public void Hide()
        {
            NameCanvas.Hide();
        }
    }
}
