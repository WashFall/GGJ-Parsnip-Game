using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioAfterActiveAudioIsDone : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioToWaitFor;
    [SerializeField] private AudioClip m_audioToPlay;

    
    void Update()
    {
        if (!m_audioToWaitFor.isPlaying)
        {
            m_audioToWaitFor.clip = m_audioToPlay;
            m_audioToWaitFor.loop = true;
            m_audioToWaitFor.Play();
            enabled = false;
        }
    }
}
