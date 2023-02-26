using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] private AudioSource Ambient;
    [SerializeField] private AudioClip GoodMusic;
    [SerializeField] private AudioClip BadMusic;

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void PlayGoodMusic()
    {
        DontDestroyOnLoad(Ambient.gameObject);
        Ambient.clip = GoodMusic;
        Ambient.Play();
    }
}
