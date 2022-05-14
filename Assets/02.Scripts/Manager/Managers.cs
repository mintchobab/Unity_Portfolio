using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    private JsonManager jsonMgr = new JsonManager();

    public JsonManager JsonMgr { get => jsonMgr; }


    public override void Init()
    {
        DontDestroyOnLoad(this);

        JsonMgr.Init();
    }
}
