using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour, Item, Interactable
{
    public int Id { get; private set; } = 3;

    void Start()
    {
        Managers.Scene.SetObjectPosition(Id, transform.position);
    }

    public void Interact(int interactingId)
    {
        Managers.Inventory.AddItem(Id);
        Messenger.Broadcast(GameEvent.ITEM_ADDED);
        Managers.Scene.DeleteObject(transform.position);
        Managers.Conditions.AddCondition("HAS_APPLE");//ЛУЧШЕ КАК-ТО ФОРМАЛИЗОВАТЬ
        Managers.Audios.PlayGoodMusic();
        Destroy(this.gameObject);
    }
}
