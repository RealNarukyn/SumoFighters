using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public enum SFXSounds { Punch, Scream, Jump, }

    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip[] sfxClips;


    private void Start()
    {
        sfxSource = this.gameObject.AddComponent<AudioSource>();
        sfxSource.volume = .5f;
    }

    public void PlaySFX(int clip)
    {
        sfxSource.PlayOneShot(sfxClips[clip]);
    }

    public void PlaySFX(int clip, float volume)
    {
        sfxSource.PlayOneShot(sfxClips[clip], volume);
    }
}
