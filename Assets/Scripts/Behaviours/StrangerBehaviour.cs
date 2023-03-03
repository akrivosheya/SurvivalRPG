using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Stranger;
    [SerializeField] private GameObject KilledStranger;
    [SerializeField] private Animator animator;

    void Awake()
    {
        Messenger.AddListener("WAKE_UP_STRANGER", OnWakeUpStranger);
        Messenger.AddListener("KILL_STRANGER", OnStrangerKilled);
        Messenger.AddListener("HAS_AXE", OnHasAxe);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener("WAKE_UP_STRANGER", OnWakeUpStranger);
        Messenger.RemoveListener("KILL_STRANGER", OnStrangerKilled);
        Messenger.RemoveListener("HAS_AXE", OnHasAxe);
    }

    public void OnStrangerKilled()
    {
        Stranger.SetActive(false);
        KilledStranger.SetActive(true);
        Messenger.Broadcast("BAD_ENDING");
        Managers.Audios.PlayHeartSound();
    }

    public void OnHasAxe()
    {
        animator.SetBool("HAS_AXE", true);
    }

    public void OnWakeUpStranger()
    {
    }
}
