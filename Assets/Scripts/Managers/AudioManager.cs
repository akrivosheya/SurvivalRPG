using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    //сделать плавные переходы
    //получать звуки из ресурсов
    public ManagerStatus status { get; private set; }
    [SerializeField] private AudioSource FontSource;
    [SerializeField] private AudioSource OneShotSource;
    [SerializeField] private List<AudioClip> Clips;

    public void Startup()
    {
        status = ManagerStatus.Started;
    }

    public void PlayOneShotSound(int soundIndex)
    {
        if(soundIndex >= Clips.Count || soundIndex < 0)
        {
            throw new NoSuchAudioClipException(soundIndex);
        }
        OneShotSource.clip = Clips[soundIndex];
        OneShotSource.Play();
    }
    
    public void StopFont()
    {
        FontSource.Stop();
    }
}

