using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Tree;

    void Awake()
    {
        Messenger.AddListener("WAKE_UP_TREE", OnTreeWakeUp);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener("WAKE_UP_TREE", OnTreeWakeUp);
    }

    public void OnTreeWakeUp()
    {
        Tree.SetActive(true);
    }
}
