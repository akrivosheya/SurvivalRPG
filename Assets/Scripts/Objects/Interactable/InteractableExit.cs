using UnityEngine;

public class InteractableExit : MonoBehaviour, Interactable
{
    [SerializeField] private Vector3 _PositionToSet;

    public void Interact(int interactingId)
    {
        Debug.Log("Interact");
        Managers.Teleport.TeleportToPosition(_PositionToSet);
    }

    public void ShowInteraction()
    {
    }
}
