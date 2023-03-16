using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class DialogPoint : MonoBehaviour, Interactable
{
    [SerializeField] private List<string> AllConditions;
    [SerializeField] private List<string> Dialogs;
    [SerializeField] private List<string> PersonsNames;
    [SerializeField] private List<string> PersonsImages;
    [SerializeField] private List<string> NewConditions;
    [SerializeField] private List<string> DeletingConditions;
    [SerializeField] private List<string> Messages;
    [SerializeField] private char SplitCharacter;
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private GameObject DialogBehaviour;
    private Behaviour _behaviour;
    private readonly string EmptyString = "";
    private ObjectData _objectData;
    private bool _showingInteraction = false;

    void Start()
    {
        _behaviour = DialogBehaviour.GetComponent<Behaviour>();
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.DialogPoint;
        var offset = Vector3.zero;
        for(int i = -Width / 2; i <= Width / 2; ++i)
        {
            for(int j = -Height / 2; j <= Height / 2; ++j)
            {
                offset.x = i;
                offset.y = j;
                offset.z = j;
                //Debug.Log("set " + name + " to " + transform.position + offset);
                Managers.Scene.SetObjectPosition((int)_objectData.Id, transform.position + offset);
            }
        }
        //Managers.Scene.SetObjectPosition((int)_objectData.Id, transform.position);
    }

    void LateUpdate()
    {
        if(_behaviour == null || Managers.Dialogs.IsDialog)
        {
            return;
        }
        if(_showingInteraction)
        {
            _behaviour.OnInteraction();
            _showingInteraction = false;
        }
        else
        {
            _behaviour.OnNoInteraction();
        }
    }

    public void ShowInteraction()
    {
        _showingInteraction = true;
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
                if(message.Equals(EmptyString))
                {
                    continue;
                }
                Messenger.Broadcast(message);
            }
            Managers.Dialogs.StartDialog(Dialogs[i].Split(SplitCharacter), PersonsNames[i].Split(SplitCharacter), PersonsImages[i].Split(SplitCharacter));
            break;
        }
    }
}
