using UnityEngine;

public class BerriesObserver : MonoBehaviour
{
    [SerializeField] private GameObject CaveDownstair;
    [SerializeField] private Vector3 PositionToSet;
    [SerializeField] private int BerriesId;
    [SerializeField] private int MaxCount = 3;

    void Awake()
    {
        Messenger.AddListener(GameEvent.GOT_BERRIES, OnGotBerries);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GOT_BERRIES, OnGotBerries);
    }
    
    public void OnGotBerries()
    {
        if(Managers.Inventory.GetCount(BerriesId) == MaxCount)
        {
            Managers.Conditions.AddCondition("HAS_ENOUGH_BERRIES");
            Managers.Teleport.TeleportToPosition(PositionToSet);
            CaveDownstair.SetActive(true);
        }
    }
}
