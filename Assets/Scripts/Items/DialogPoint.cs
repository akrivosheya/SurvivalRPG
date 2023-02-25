using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPoint : MonoBehaviour, Interactable
{
    public int Id { get; private set; } = 5;//лучше где-то взять
    [SerializeField] private List<string> AllConditions;
    [SerializeField] private List<string> Dialogs;
    [SerializeField] private List<string> NewConditions;
    [SerializeField] private List<string> DeletingConditions;

    void Start()
    {
        Managers.Scene.SetObjectPosition(Id, transform.position);
    }

    public void Interact(int interactingId)
    {
        for(int i = 0; i < AllConditions.Count; ++i)
        {
            var conditionsString = AllConditions[i];
            var conditionsArray = conditionsString.Split(';');//константа
            bool correctDialog = true;
            foreach(string condition in conditionsArray)
            {
                Debug.Log("Index: " + i + "; condition: " + condition.Length);
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
            foreach(var condition in NewConditions[i].Split(';'))//константа
            {
                Managers.Conditions.AddCondition(condition);
            }
            foreach(var condition in DeletingConditions[i].Split(';'))//константа
            {
                Managers.Conditions.DeleteCondition(condition);
            }
            Managers.Dialogs.StartDialog(Dialogs[i].Split(';'));//константа
            break;
        }
    }
}
