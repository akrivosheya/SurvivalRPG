using UnityEngine;

public class StrangerBehaviour : MonoBehaviour, Behaviour
{
    [SerializeField] private GameObject Stranger;
    [SerializeField] private GameObject KilledStranger;
    [SerializeField] private Animator animator;

    void Awake()
    {
        Messenger.AddListener(GameEvent.KILL_STRANGER, OnStrangerKilled);
        Messenger.AddListener(GameEvent.GOT_AXE, OnGotAxe);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.KILL_STRANGER, OnStrangerKilled);
        Messenger.RemoveListener(GameEvent.GOT_AXE, OnGotAxe);
    }

    public void OnInteraction()
    {
        animator.SetBool("IS_INTERACTABLE", true);
    }

    public void OnNoInteraction()
    {
        animator.SetBool("IS_INTERACTABLE", false);
    }

    public void OnStrangerKilled()
    {
        Stranger.SetActive(false);
        KilledStranger.SetActive(true);
        Managers.Audios.PlayOneShotSound(AudioClipsId.HEARTBEAT);
    }

    public void OnGotAxe()
    {
        animator.SetBool("NO_AXE", true);
    }
}
