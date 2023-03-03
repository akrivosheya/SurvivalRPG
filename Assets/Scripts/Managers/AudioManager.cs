using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] private AudioSource Ambient;
    [SerializeField] private AudioSource OneShot;
    [SerializeField] private AudioClip WindSound;
    [SerializeField] private AudioClip AxeSound;
    [SerializeField] private AudioClip HeartSound;
    [SerializeField] private AudioClip ScarySound;
    [SerializeField] private AudioClip TreeSound;

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void PlayAxeSound()
    {
        OneShot.clip = AxeSound;
        OneShot.Play();
    }

    public void PlayScarySound()
    {
        OneShot.clip = ScarySound;
        OneShot.Play();
    }

    public void PlayHeartSound()
    {
        OneShot.clip = HeartSound;
        OneShot.Play();
    }

    public void PlayTreeSound()
    {
        OneShot.clip = TreeSound;
        OneShot.Play();
    }
    
    public void StopAmbient()
    {
        Ambient.Stop();
    }
}

