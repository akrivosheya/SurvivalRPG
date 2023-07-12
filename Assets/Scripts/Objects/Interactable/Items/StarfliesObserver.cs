using System.Collections.Generic;
using UnityEngine;

public class StarfliesObserver : MonoBehaviour
{
    [SerializeField] private List<GameObject> Starflies;
    [SerializeField] private int StarfliesId;
    [SerializeField] private int MaxCount = 2;

    void Awake()
    {
        Messenger.AddListener(GameEvent.START_SEARCH_FIREFLY, OnStartSearchFirefly);
        Messenger.AddListener(GameEvent.GOT_STARFLY, OnGotStarFly);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.START_SEARCH_FIREFLY, OnStartSearchFirefly);
        Messenger.RemoveListener(GameEvent.GOT_STARFLY, OnGotStarFly);
    }
    
    public void OnGotStarFly()
    {
        if(Managers.Inventory.GetCount(StarfliesId) == MaxCount)
        {
            Managers.Conditions.AddCondition("HAS_ENOUGH_STARFLIES");
        }
    }
    
    public void OnStartSearchFirefly()
    {
        foreach(var starfly in Starflies)
        {
            starfly.SetActive(true);
        }
    }
}
