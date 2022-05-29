using System;
using System.Collections.Generic;

[Serializable]
public class JsonString
{
    public List<StringKeyvalues> stringNpcNames;
    public List<StringKeyvalues> stringDialogues;
    public List<StringKeyvalues> stringQuestNames;
    public List<StringKeyvalues> stringQuestDescriptions;
    public List<StringKeyvalues> stringQuestDialogues;
    public List<StringKeyvalues> stringItemNames;
    public List<StringKeyvalues> stringItemDescriptions;
    public List<StringKeyvalues> stringUITexts;
}


[Serializable]
public class StringKeyvalues
{
    public string key;
    public List<string> values;
}
