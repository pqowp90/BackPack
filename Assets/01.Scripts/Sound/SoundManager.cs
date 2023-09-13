using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public enum AudioType
{
    MUSIC,
    SFX,
    Length,
}
public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private List<SoundContainer> _soundContainers = new List<SoundContainer>();
    public List<SoundContainer> SoundContainers => _soundContainers;
    public Dictionary<SoundEnum, SoundContainer> enumDictionary = new Dictionary<SoundEnum, SoundContainer>();
    private SoundObj musicAudioSources;
    [SerializeField]
    private List<AudioMixerGroup> audioMixerGroups = new List<AudioMixerGroup>();
    private List<SoundObj> audioSources = new List<SoundObj>();

    Dictionary<string, AudioMixerGroup> audioMixerGroupDic = new Dictionary<string, AudioMixerGroup>();
    protected override void Start()
    {
        base.Start();
        
        for (int i = 0; i < 5; i++)
        {
            CreateSoundObj();
        }
        musicAudioSources = GetMusicSources();
        musicAudioSources.gameObject.SetActive(true);
    }
    private void Awake() {
        AudioMixer mixer = Resources.Load("AudioMixer") as AudioMixer;
        audioMixerGroups.Clear();
        foreach (AudioType audioType in Enum.GetValues(typeof(AudioType)))
        {
            AudioMixerGroup[] groups = mixer.FindMatchingGroups(audioType.ToString());
            if(groups.Length > 0)
            {
                audioMixerGroups.Add(groups[0]);
            }
        }
        foreach (var audioMixer in audioMixerGroups)
        {
            audioMixerGroupDic[audioMixer.name] = audioMixer;
        }
    }
    private SoundObj CreateSoundObj()
    {
        GameObject audioSource = new GameObject("audioSource");
        audioSource.transform.position = Vector3.one * 100f;
        audioSource.AddComponent<AudioSource>();
        audioSource.transform.SetParent(transform);
        SoundObj soundObj = audioSource.AddComponent<SoundObj>();
        audioSources.Add(soundObj);
        soundObj.gameObject.SetActive(false);
        return soundObj;
    }
    private SoundObj GetSoundObj()
    {
        foreach (var soundObj in audioSources)
        {
            if(soundObj.playing == false)
            {
                return soundObj;
            }
        }
        if(audioSources.Count < 32)
        {
            return CreateSoundObj();
        }
        SoundObj audioSource = audioSources[0];
        audioSources.Remove(audioSource);
        audioSources.Add(audioSource);
        
        return audioSource;
    }
    private SoundObj GetMusicSources()
    {
        SoundObj soundObj = GetSoundObj();
        musicAudioSources = soundObj;
        audioSources.Remove(soundObj);
        return soundObj;
    }




    public void PlaySound(AudioClip audioClip, Vector3 pos, float volume, float distance, AudioType audioMixerType)
    {
        if(!audioClip) return;
        SoundObj soundObj = GetSoundObj();
        soundObj.gameObject.SetActive(true);
        soundObj.PlaySound(audioClip, pos, volume, distance, audioMixerGroupDic[audioMixerType.ToString()]);
    }
    private SoundContainer GetSoundAsset(SoundEnum soundEnum)
    {
        SoundContainer _soundContainer;
        if(enumDictionary.TryGetValue(soundEnum, out _soundContainer))
        {
            return _soundContainer;
        }
        
        _soundContainer = _soundContainers.Find((x)=>x.audioClip.name.Replace(" ", string.Empty) == soundEnum.ToString());
        enumDictionary.Add(soundEnum, _soundContainer);
        return _soundContainer;
    }
    public void PlaySound(SoundEnum soundEnum, Vector3 pos, float volume, float distance)
    {
        

        SoundContainer soundContainer = GetSoundAsset(soundEnum);

        if(soundContainer == null)
            return;
        SoundObj soundObj = GetSoundObj();
        soundObj.gameObject.SetActive(true);
        soundObj.PlaySound(soundContainer.audioClip, pos, volume, distance, audioMixerGroupDic[soundContainer.soundType.ToString()]);
    }

    public void PlayBGM(SoundEnum soundEnum)
    {

        SoundContainer soundContainer = GetSoundAsset(soundEnum);
        
        if(soundContainer == null) return;
        musicAudioSources.PlayMusic(soundContainer.audioClip, audioMixerGroupDic[AudioType.MUSIC.ToString()]);
    }
    public void StopBGM()
    {
        musicAudioSources.StopMusic();
    }
    public void SetBGMVolum(float volum)
    {
        musicAudioSources.SetVolum(volum);
    }
    public void SetBGMTime(float volum)
    {
        musicAudioSources.SetTime(volum);
    }

    public void ResetSoundContainer()
    {
        _soundContainers.Clear();
        enumDictionary.Clear();
    }
    public void AddSoundContainer(SoundContainer container)
    {
        _soundContainers.Add(container);
    }
}
