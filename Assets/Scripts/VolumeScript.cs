using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    //Sets volume level
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
    }
}
