using System.Collections.Generic;
using UnityEngine;

public class UIItem : MonoBehaviour
{
    [SerializeField] private string Dialogs;
    [SerializeField] private string PersonsNames;
    [SerializeField] private string PersonsImages;
    [SerializeField] private char SplitCharacter;
    private string[] EmptyArray = new string[0];

    public void OnClick()
    {
        if(Managers.Dialogs.IsDialog || Managers.Conditions["SCENE_IS_CHANGING"] || Managers.Conditions["IS_PAUSE"])
        {
            return;
        }
        Managers.Dialogs.StartDialog(Dialogs.Split(SplitCharacter), PersonsNames.Split(SplitCharacter), PersonsImages.Split(SplitCharacter),
            EmptyArray, EmptyArray, EmptyArray);
    }
}
