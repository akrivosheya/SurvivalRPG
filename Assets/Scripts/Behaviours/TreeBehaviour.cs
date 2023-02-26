using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Tree;
    [SerializeField] private GameObject DeadTree;
    [SerializeField] private Animator animator;
    [SerializeField] private List<GameObject> Skulls;
    [SerializeField] private float SkullsSpeed;
    private bool _skullsAwaken = false;
    private bool _hasToAwakeSkulls = false;

    void Awake()
    {
        Messenger.AddListener("WAKE_UP_TREE", OnTreeWakeUp);
        Messenger.AddListener("KILL_TREE", OnTreeKill);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener("WAKE_UP_TREE", OnTreeWakeUp);
        Messenger.AddListener("KILL_TREE", OnTreeKill);
    }

    public void OnTreeWakeUp()
    {
        animator.SetBool("SPOKEN_TO_STRANGER", true);
    }

    public void OnTreeKill()
    {
        Tree.SetActive(false);
        DeadTree.SetActive(true);
        _hasToAwakeSkulls = true;
        Messenger.Broadcast("GOOD_ENDING");
        Managers.Audios.PlayTreeSound();
    }

    void Update()
    {
        if(_hasToAwakeSkulls && !_skullsAwaken)
        {
            _skullsAwaken = true;
            foreach(var skull in Skulls)
            {
                var renderer = skull.GetComponent<SpriteRenderer>();
                var color = renderer.color;
                color.a += Time.deltaTime * SkullsSpeed;
                renderer.color = color;
                if(color.a < 1)
                {
                    _skullsAwaken = false;
                }
            }
        }
    }
}
