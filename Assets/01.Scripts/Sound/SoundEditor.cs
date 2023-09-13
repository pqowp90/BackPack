using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

using UnityEngine;
using System;
using UnityEngine.Audio;


#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SoundManager))]


public class SoundEditor : Editor
{
    private static readonly string scriptPath = "Assets/01.Scripts/Sound/SoundEnum.cs";
    private static readonly string enumName = "SoundEnum";
    private const string SOUND_CLIP_PATH = "Assets/07.Sounds";
    private SoundManager _soundManager;
    public override void OnInspectorGUI()
    {
        if(Application.isPlaying) return;
        _soundManager = (SoundManager)target;
        GUILayout.BeginHorizontal();
        {
            if(GUILayout.Button("Refresh"))
            {
                GenerateSoundEnum();
                _soundManager.ResetSoundContainer();
                List<AudioClip> soundTexts = GetSoundsInProject();
                for (int i = 0; i < soundTexts.Count; ++i)
                {
                    int index = i;
                    AudioType audioType = AudioType.SFX;
                    if(soundTexts[i].length > 30f)
                    {
                        audioType = AudioType.MUSIC;
                    }
                    _soundManager.AddSoundContainer(new SoundContainer(soundTexts[i], audioType)); 
                }
                EditorUtility.SetDirty(_soundManager);
            }
        }
        EditorUtility.SetDirty(target);

        UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);

        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }

    private List<AudioClip> GetSoundsInProject()
    {
        var textList = AssetDatabase.FindAssets("t:AudioClip" , new []{SOUND_CLIP_PATH}).ToList()
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<AudioClip>)
            .ToList();
        return textList;
    }
    
    //[MenuItem("Custom/Generate Sound ENUM")]
    private void GenerateSoundEnum()
    {
        List<AudioClip> sounds = GetSoundsInProject();

        

        string enumCode = "public enum " + enumName + "\n{\n";

        for (int i = 0; i < sounds.Count; i++)
        {
            string soundName = sounds[i].name;
            string enumItemName = soundName.Replace(" ", string.Empty);
            enumCode += "    " + enumItemName + " = " + i + ",\n";
        }
        enumCode += "    None,\n";

        enumCode += "}\n";

        ModifyScript(enumCode);
    }
    private static void ModifyScript(string enumCode)
    {
        string scriptText = File.ReadAllText(scriptPath);

        // 특정 패턴을 찾아 ENUM 코드로 변경
        string modifiedScriptText = Regex.Replace(scriptText, @"public enum SoundEnum\s*\{[^}]*\}", enumCode);

        File.WriteAllText(scriptPath, modifiedScriptText);

        AssetDatabase.Refresh();
    }
}
#endif