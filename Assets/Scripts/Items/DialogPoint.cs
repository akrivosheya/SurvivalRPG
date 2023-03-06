using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class DialogPoint : MonoBehaviour, Interactable
{
    [SerializeField] private List<string> AllConditions;
    [SerializeField] private List<string> Dialogs;
    [SerializeField] private List<string> Persons;
    [SerializeField] private List<string> NewConditions;
    [SerializeField] private List<string> DeletingConditions;
    [SerializeField] private List<string> Messages;
    [SerializeField] private char SplitCharacter;
    private ObjectData _objectData;

    void Start()
    {
        _objectData.Id = ObjectsId.DialogPoint;
        Managers.Scene.SetObjectPosition((int)_objectData.Id, transform.position);
    }

    public void Interact(int interactingId)
    {
        for(int i = 0; i < AllConditions.Count; ++i)
        {
            var conditionsString = AllConditions[i];
            var conditionsArray = conditionsString.Split(SplitCharacter);
            bool correctDialog = true;
            foreach(string condition in conditionsArray)
            {
                if(!Managers.Conditions[condition])
                {
                    correctDialog = false;
                    break;
                }
            }
            if(conditionsArray.Length >= 1 && conditionsArray[0].Length > 0 && !correctDialog)
            {
                continue;
            }
            foreach(var condition in NewConditions[i].Split(SplitCharacter))
            {
                Managers.Conditions.AddCondition(condition);
            }
            foreach(var condition in DeletingConditions[i].Split(SplitCharacter))
            {
                Managers.Conditions.DeleteCondition(condition);
            }
            foreach(var message in Messages[i].Split(SplitCharacter))
            {
                Messenger.Broadcast(message);
            }
            Managers.Dialogs.StartDialog(Dialogs[i].Split(SplitCharacter), Persons[i].Split(SplitCharacter));
            break;
        }
    }
}
