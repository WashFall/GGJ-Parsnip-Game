using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public void Set(float sliderValue)
    {
        VolumeData.volume = sliderValue;
    }
}
