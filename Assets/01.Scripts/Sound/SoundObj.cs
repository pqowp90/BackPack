using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class SoundObj : MonoBehaviour
{
    private Coroutine coroutine;
    public bool playing = false;
    private AudioSource _audioSource;
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 1f;
        _audioSource.playOnAwake = false;
    }
    public void PlaySound(AudioClip audioClip, Vector3 pos, float volume, float distance, AudioMixerGroup audioMixerGroup)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        playing = true;
        transform.position = pos;
        _audioSource.minDistance = distance;
        _audioSource.minDistance = 1;
        _audioSource.maxDistance = 20;
        _audioSource.rolloffMode = AudioRolloffMode.Linear;
        _audioSource.volume = volume;
        _audioSource.outputAudioMixerGroup = audioMixerGroup;
        _audioSource.Stop();
        _audioSource.clip = audioClip;
        _audioSource.Play();
        coroutine = StartCoroutine(WaitForSoundEnd(audioClip.length));

        Transform2Dor3D(audioMixerGroup.name);
        
    }
    public void PlayMusic(AudioClip audioClip, AudioMixerGroup audioMixerGroup)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        playing = true;
        transform.position = Vector3.zero;
        _audioSource.minDistance = 1;
        _audioSource.maxDistance = 20;
        _audioSource.loop = true;
        _audioSource.rolloffMode = AudioRolloffMode.Linear;
        _audioSource.volume = 0f;
        
        _audioSource.outputAudioMixerGroup = audioMixerGroup;
        _audioSource.Stop();
        _audioSource.clip = audioClip;
        _audioSource.Play();

        _audioSource.DOFade(0.5f, 1f);

        coroutine = StartCoroutine(WaitForSoundEnd(audioClip.length));

        Transform2Dor3D("MUSIC");
    }
    public void StopMusic()
    {
        _audioSource.DOFade(0f, 1f);
    }
    public void SetVolum(float volum)
    {
        _audioSource.DOKill();
        _audioSource.DOFade(volum, 1f);
    }
    public void SetTime(float time)
    {
        _audioSource.time = time;
    }
    private void Transform2Dor3D(string audioMixerGroupName)
    {
        if (Enum.IsDefined(typeof(AudioType), audioMixerGroupName))
        {
            switch ((AudioType)Enum.Parse(typeof(AudioType), audioMixerGroupName))
            {
                case AudioType.MUSIC:
                    _audioSource.spatialBlend = 0f;
                    break;
                case AudioType.SFX:
                    _audioSource.spatialBlend = 1f;
                    break;
            }
        }
    }
    private IEnumerator WaitForSoundEnd(float time)
    {
        yield return new WaitForSeconds(time);
        playing = false;
        gameObject.SetActive(playing);
    }
}