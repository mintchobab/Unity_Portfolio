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
            if (localizeIndex == 0)
                return Tables.LanguageTable[id].Korean;
            else
                return Tables.LanguageTable[id].English;
        }
    }
}