using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Win") return;
            AudioSource.PlayClipAtPoint(clip,camera.position);
    }
}
