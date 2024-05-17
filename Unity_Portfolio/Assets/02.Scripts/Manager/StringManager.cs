using System.Diagnostics;

namespace lsy
{
    public class StringManager : IManager
    {
        private static int localizeIndex = 0;

        public void Init()
        {
            string language = UnityEngine.PlayerPrefs.GetString("Language");
            localizeIndex = language.Equals("Korean") ? 0 : 1;
        }

        public static string Get(string id)
        {
            //UnityEngine.Debug.LogWarning("String ID : " + id);

            return Tables.LanguageTable[id].Korean;
        }
    }
}
