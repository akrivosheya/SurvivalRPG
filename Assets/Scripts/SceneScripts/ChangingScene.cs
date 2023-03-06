using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingScene : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Font;
    [SerializeField] private float BlurSpeed;

    void Update()
    {
        if(!Managers.Dialogs.IsDialog && Managers.Conditions["START_ENDING"])
        {
            if(Font != null)
            {
                var color = Font.color;
                color.a += Time.deltaTime * BlurSpeed;
                Font.color = color;
            }
            if(Font == null || Font.color.a >= 1)
            {
                if(Managers.Conditions["IS_BAD_ENDING"])
                {
                    Managers.Levels.LoadScene("BadEnding");
                }
                else if(Managers.Conditions["IS_GOOD_ENDING"])
                {
                    Managers.Levels.LoadScene("GoodEnding");
                }
            }
        }
    }
}
