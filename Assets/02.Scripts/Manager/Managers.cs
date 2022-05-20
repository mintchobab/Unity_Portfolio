using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    private JsonManager jsonManager = new JsonManager();
    private StringManager stringManager = new StringManager();
    private ResourceManager resourceManager = new ResourceManager();
    private UIManager uiManger = new UIManager();
    private QuestManager questManager = new QuestManager();

    public JsonManager JsonManager { get => jsonManager; }
    public StringManager StringManager { get => stringManager; }
    public ResourceManager ResourceManager { get => resourceManager; }
    public UIManager UIManager { get => uiManger; }
    public QuestManager QuestManager { get => questManager; }


    public override void Init()
    {
        DontDestroyOnLoad(this);

        JsonManager.Init();
        StringManager.Init();
        ResourceManager.Init();
        UIManager.Init();
        QuestManager.Init();
    }
}
