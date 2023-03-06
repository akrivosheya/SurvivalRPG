using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Tree;
    [SerializeField] private GameObject DeadTree;
    [SerializeField] private Animator animator;

    void Awake()
    {
        Messenger.AddListener("WAKE_UP_TREE", OnTreeWakeUp);
        Messenger.AddListener("KILL_TREE", OnTreeKill);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener("WAKE_UP_TREE", OnTreeWakeUp);
        Messenger.RemoveListener("KILL_TREE", OnTreeKill);
    }

    public void OnTreeWakeUp()
    {
        animator.SetBool("AWAKEN", true);
    }

    public void OnTreeKill()
    {
        Tree.SetActive(false);
        DeadTree.SetActive(true);
        Managers.Audios.PlayOneShotSound(AudioClipsId.TREE);
    }
}
