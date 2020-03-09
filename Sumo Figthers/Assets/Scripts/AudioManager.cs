using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum MusicClips { menu, game_loop }
    public enum SFXClips { button }
    

    #region Static Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion


    #region Fields
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    [SerializeField]
    private AudioClip[] musicClips;
    [SerializeField]
    private AudioClip[] sfxClips;

    private bool firstMusicSourceIsPlaying;
    #endregion


    private void Awake()
    {
        // Make sure we don't destroy this instance
        DontDestroyOnLoad(this.gameObject);

        // Create audio sources, and save them as references
        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        // Loop the music tracks
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(int clip)
    {
        // Determine which source is active
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        activeSource.clip = musicClips[clip];
        activeSource.volume = 0.75f;
        activeSource.Play();
    }

    public void PlaySFX(int clip)
    {
        sfxSource.PlayOneShot(sfxClips[clip]);
    }
    public void PlaySFX(int clip, float volume)
    {
        sfxSource.PlayOneShot(sfxClips[clip], volume);
    }


    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;        
    }

}
