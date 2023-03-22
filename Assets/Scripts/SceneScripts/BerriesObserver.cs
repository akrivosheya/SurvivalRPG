using UnityEngine;

public class BerriesObserver : MonoBehaviour
{
    [SerializeField] private MovingPlayer Player;
    [SerializeField] private Vector3 PositionToSet;
    [SerializeField] private int BerriesId;
    [SerializeField] private int MaxCount = 3;

    void Awake()
    {
        Messenger.AddListener(GameEvent.GOT_BERRIES, OnGotBerries);
        Messenger.AddListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GOT_BERRIES, OnGotBerries);
        Messenger.RemoveListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }
    
    public void OnGotBerries()
    {
        if(Managers.Inventory.GetCount(BerriesId) == MaxCount)
        {
            Messenger.Broadcast(GameEvent.NEED_BLACK_SCENE);
        }
    }
    
    public void OnBlackScene()
    {
        Player.SetTo(PositionToSet);
        Messenger.Broadcast(GameEvent.NEED_CLEAR_SCENE);
    }
}
