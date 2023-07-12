using UnityEngine;

public class TreeBehaviour : MonoBehaviour, Behaviour
{
    [SerializeField] private GameObject Tree;
    [SerializeField] private GameObject Eyes;
    [SerializeField] private GameObject DeadTree;
    [SerializeField] private Animator animator;

    void Awake()
    {
        Messenger.AddListener(GameEvent.WAKE_UP_TREE, OnTreeWakeUp);
        Messenger.AddListener(GameEvent.KILL_TREE, OnTreeKill);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.WAKE_UP_TREE, OnTreeWakeUp);
        Messenger.RemoveListener(GameEvent.KILL_TREE, OnTreeKill);
    }

    public void OnInteraction()
    {
        animator.SetBool("IS_INTERACTABLE", true);
    }

    public void OnNoInteraction()
    {
        animator.SetBool("IS_INTERACTABLE", false);
    }

    public void OnTreeWakeUp()
    {
        animator.SetBool("AWAKEN", true);
        Eyes.SetActive(true);
    }

    public void OnTreeKill()
    {
        Tree.SetActive(false);
        DeadTree.SetActive(true);
        Managers.Audios.PlayOneShotSound(AudioClipsId.TREE);
    }
}
