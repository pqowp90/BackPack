using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
}
[CreateAssetMenu(menuName = "SO/Sound")]
public class SoundListSO : ScriptableObject
{
    public List<Sound> Sounds = new List<Sound>();
}
