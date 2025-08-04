using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isPlayerAudio = false;
    public AudioSource[] sfxSource;
    public AudioSource[] musicSource;
    void Start()
    {
        if (isPlayerAudio)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                PlaySFX(0);
            }
            else
            {
                PlayMusic(1);
            }
        }
    }
    public void StopAllSFX()
    {
        foreach (AudioSource a in sfxSource)
        {
            a.Stop();
        }
    }
    public void PlayHumanWalkSFX()
    {
        PlaySFX(Random.Range(4, 7));
    }
    public void PlaySFX(int index)
    {
        sfxSource[index].Play();
    }
    public void PlayMusic(int index)
    {
        foreach (AudioSource a in musicSource)
        {
            a.Stop();
        }
        
        musicSource[index].Play();
    }
}
