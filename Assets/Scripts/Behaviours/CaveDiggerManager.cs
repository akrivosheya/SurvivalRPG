using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveDiggerManager : MonoBehaviour
{
    void Awake()
    {
        Messenger.AddListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }

    public void OnBlackScene()
    {
        if(Managers.Conditions["ESCAPE_FOREST"])
        {
            Managers.Levels.LoadScene("EscapeForest");
        }
    }
}
