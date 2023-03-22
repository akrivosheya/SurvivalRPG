using UnityEngine;

public class ThingBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Thing;

    void Awake()
    {
        Messenger.AddListener(GameEvent.WAKE_UP_THING, OnThingWakeUp);
        Messenger.AddListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.WAKE_UP_THING, OnThingWakeUp);
        Messenger.RemoveListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }

    public void OnThingWakeUp()
    {
        Thing.SetActive(true);
    }

    public void OnBlackScene()
    {
        if(Managers.Conditions["START_ENDING"])
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
