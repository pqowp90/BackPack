using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
[Serializable]
class AudioGroup
    {
        public List<AudioClip> audioClips;
    }
public class SoundEventTrigger : MonoBehaviour
{
    
    [SerializeField] private float volum = 1;
    [SerializeField] private float distance = 1;
    [SerializeField] List<AudioGroup> AudioGroups = new List<AudioGroup>();
    protected void PlaySound(AudioClip audioClip)
    {
        SoundManager.Instance.PlaySound(audioClip, transform.position, volum, distance, AudioType.SFX);
    }

    protected void PlaySoundAtGroup(int index)
    {
        if(AudioGroups.Count <= index) return;
        
        
        AudioClip audio = AudioGroups[index].audioClips[UnityEngine.Random.Range(0, AudioGroups[index].audioClips.Count)];

        PlaySound(audio);
    }


}
    
