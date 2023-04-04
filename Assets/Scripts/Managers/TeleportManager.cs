using UnityEngine;

public class TeleportManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] private MovingPlayer Player;
    private Vector3 _PositionToTeleport;

    void Awake()
    {
        Messenger.AddListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.BLACK_SCENE, OnBlackScene);
    }

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void TeleportToPosition(Vector3 position)
    {
        _PositionToTeleport = position;
        Messenger.Broadcast(GameEvent.NEED_BLACK_SCENE);
    }
    
    public void OnBlackScene()
    {
        if(Managers.Conditions["START_ENDING"])
        {
            return;
        }
        Player.SetTo(_PositionToTeleport);
        Messenger.Broadcast(GameEvent.NEED_CLEAR_SCENE);
    }
}
