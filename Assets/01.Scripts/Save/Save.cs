using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Save))]
public class SaveEditor : Editor
{
    Save save;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Save"))
        {
            save = (Save)target;
            save.SaveAll();
        }
        if(save)
        {
            save = (Save)target;
            save.SaveAll();
        }
    }
}
#endif

public class Save : MonoBehaviour
{
    public SaveFile saveFile;
    public DateTime saveTime = DateTime.Now;
    private string fileName;
    public void SaveAll() {
        saveTime = DateTime.Now;
        fileName = saveTime.ToString("yyyy-MM-dd hh:mm:ss");
        saveFile.FileName = fileName;
    }
}
