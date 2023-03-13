using UnityEngine;

public class ThingBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Thing;

    void Awake()
    {
        Messenger.AddListener(GameEvent.WAKE_UP_THING, OnThingWakeUp);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.WAKE_UP_THING, OnThingWakeUp);
    }

    public void OnThingWakeUp()
    {
        Thing.SetActive(true);
    }
}
