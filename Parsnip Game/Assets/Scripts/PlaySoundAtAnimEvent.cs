using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtAnimEvent : MonoBehaviour
{
    public AudioClip clip;
    public Transform camera;
    public AudioSource source;

    private void Start()
    {
        camera = Camera.main.transform;
    }

    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(clip,camera.position);
    }
}
