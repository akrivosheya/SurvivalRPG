using UnityEngine;

public class StrangerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Stranger;
    [SerializeField] private GameObject KilledStranger;
    [SerializeField] private Animator animator;

    void Awake()
    {
        Messenger.AddListener("KILL_STRANGER", OnStrangerKilled);
        Messenger.AddListener("GOT_AXE", OnGotAxe);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener("KILL_STRANGER", OnStrangerKilled);
        Messenger.RemoveListener("GOT_AXE", OnGotAxe);
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
