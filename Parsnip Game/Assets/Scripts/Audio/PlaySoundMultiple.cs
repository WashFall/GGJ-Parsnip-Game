using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundMultiple : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    private string audioName;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string audioName)
    {
        this.audioName = audioName;
        audioSource.volume = VolumeData.volume;
        audioSource.PlayOneShot(audioClips.First(clip => clip.name.Equals(this.audioName)));
    }
}
