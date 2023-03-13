using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectData))]

public class SelectingObject : MonoBehaviour, Item, Interactable
{
    public int Id { get; private set; } = 3;
    [SerializeField] private List<string> Conditions;
    [SerializeField] private List<string> AddingConditions;
    [SerializeField] private List<string> BroadcastMessages;
    [SerializeField] private int PlayingSoundId;
    [SerializeField] private bool HasToBeDeleted;
    private ObjectData _objectData;

    void Start()
    {
        _objectData = GetComponent<ObjectData>();
        _objectData.Id = ObjectsId.Selecting;
        Managers.Scene.SetObjectPosition((int)_objectData.Id, transform.position);
    }

    public void ShowInteraction()
    {
    }

    public void Interact(int interactingId)
    {
        var canBeInteracted = true;
        foreach(var condition in Conditions)
        {
            if(!Managers.Conditions[condition])
            {
                canBeInteracted = false;
                break;
            }
        }
        if(canBeInteracted)
        {
            Managers.Inventory.AddItem(Id);
            Messenger.Broadcast(GameEvent.ITEM_ADDED);
            if(HasToBeDeleted)
            {
                Managers.Scene.DeleteObject(transform.position);
            }
            foreach(var condition in AddingConditions)
            {
                Managers.Conditions.AddCondition(condition);
            }
            foreach(var message in BroadcastMessages)
            {
                Messenger.Broadcast(message);
            }
            Managers.Audios.PlayOneShotSound(PlayingSoundId);
            Destroy(this.gameObject);
        }
    }
}
