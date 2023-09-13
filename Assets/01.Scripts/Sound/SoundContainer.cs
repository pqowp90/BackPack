using System;
using UnityEngine;

[Serializable]
public class SoundContainer
{
    public AudioType soundType;
    
    public AudioClip audioClip;
    
    public SoundContainer(AudioClip soundClip, AudioType soundType)
    {
        this.audioClip = soundClip;
        this.soundType = soundType;
    }
}
