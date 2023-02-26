using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Thing;
    [SerializeField] private SpriteRenderer Font;
    [SerializeField] private float BlurSpeed;
    private bool _badEnding = false;
    private bool _goodEnding = false;

    void Awake()
    {
        Messenger.AddListener("WAKE_UP_THING", OnThingWakeUp);
        Messenger.AddListener("BAD_ENDING", OnBadEnding);
        Messenger.AddListener("GOOD_ENDING", OnGoodEnding);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener("WAKE_UP_THING", OnThingWakeUp);
        Messenger.RemoveListener("GOOD_ENDING", OnGoodEnding);
        Messenger.RemoveListener("BAD_ENDING", OnBadEnding);
    }

    public void OnThingWakeUp()
    {
        Thing.SetActive(true);
    }

    public void OnBadEnding()
    {
        _badEnding = true;
        Managers.Conditions.AddCondition("IS_ENDING");
    }

    public void OnGoodEnding()
    {
        _goodEnding = true;
        Managers.Conditions.AddCondition("IS_ENDING");
    }

    void Update()
    {
        if(!Managers.Dialogs.IsDialog && Managers.Conditions["START_ENDING"])
        {
            var color = Font.color;
            color.a += Time.deltaTime * BlurSpeed;
            Font.color = color;
            if(color.a >= 1)
            {
                if(_badEnding)
                {
                    Managers.Levels.LoadScene("BadEnding");
                }
                else if(_goodEnding)
                {
                    Managers.Levels.LoadScene("GoodEnding");
                }
            }
        }
    }
}
