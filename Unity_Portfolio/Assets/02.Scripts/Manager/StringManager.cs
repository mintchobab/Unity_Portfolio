namespace lsy
{
    public class StringManager : IManager
    {
        private static JsonString jsonString;

        private static int localizeIndex = 0;

        public void Init()
        {
            jsonString = Managers.Instance.JsonManager.jsonString;

            string language = UnityEngine.PlayerPrefs.GetString("Language");
            localizeIndex = language.Equals("Korean") ? 0 : 1;
        }

        public static string Get(string id)
        {
            return Tables.LanguageTable[id].Korean;
        }




        public static string GetLocalizedItemName(string nameKey)
        {
            return jsonString.stringItemNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedItemExplanation(string nameKey)
        {
            return jsonString.stringItemExplanations.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }








        public static string GetLocalizedQuestName(string nameKey)
        {
            return jsonString.stringQuestNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestGoal(string nameKey)
        {
            return jsonString.stringQuestGoals.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestContent(string nameKey)
        {
            return jsonString.stringQuestContents.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestDialogue(string nameKey)
        {
            return jsonString.stringQuestDialogues.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedUIText(string nameKey)
        {
            return jsonString.stringUITexts.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedSystemMessage(string nameKey)
        {
            return jsonString.stringSystemMessages.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedSkillName(string namekey)
        {
            return jsonString.stringSkillNames.Find(x => x.key.Equals(namekey)).values[localizeIndex];
        }
    }
}
