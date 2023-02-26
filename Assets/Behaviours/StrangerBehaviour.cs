using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Stranger;
    [SerializeField] private GameObject KilledStranger;

    void Awake()
    {
        Messenger.AddListener("KILL_STRANGER", OnStrangerKilled);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener("KILL_STRANGER", OnStrangerKilled);
    }

    public void OnStrangerKilled()
    {
        Stranger.SetActive(false);
        KilledStranger.SetActive(true);
    }
}
