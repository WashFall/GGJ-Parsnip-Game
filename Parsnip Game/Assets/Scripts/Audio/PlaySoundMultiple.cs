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

        switch (audioName)
        {
            case "Parsnip big":
                audioSource.PlayOneShot(audioClips[1]);
                break;
            case "Roots ":
                audioSource.PlayOneShot(audioClips[3]);
                break;
            case "Farmer":
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "Farmer 2":
                audioSource.PlayOneShot(audioClips[1]);
                break;
            case "Farmer 3":
                audioSource.PlayOneShot(audioClips[2]);
                break;
            
        }
    }
}
