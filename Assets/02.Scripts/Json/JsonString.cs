using System;
using System.Collections.Generic;

[Serializable]
public class JsonString
{
    public List<StringKeyvalues> stringNpcNames;
    public List<StringKeyvalues> stringNpcDialogues;

    public List<StringKeyvalues> stringQuestNames;
    public List<StringKeyvalues> stringQuestGoals;
    public List<StringKeyvalues> stringQuestContents;
    public List<StringKeyvalues> stringQuestDialogues;

    public List<StringKeyvalues> stringItemNames;
    public List<StringKeyvalues> stringItemExplanations;

    public List<StringKeyvalues> stringUITexts;
    public List<StringKeyvalues> stringSystemMessages;
    public List<StringKeyvalues> stringSkillNames;
    //public List<StringKeyvalues> stringCollection;
}


[Serializable]
public class StringKeyvalues
{
    public string key;
    public List<string> values;
}
