#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;


[InitializeOnLoad]
public class ToolbarPlayerPrefab
{
    private const string PLAYER_FILE_PATH = "Assets/Resources/Prefabs/Player.prefab";
    static ToolbarPlayerPrefab()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        
    }
    private static void OnToolbarGUI()
    {
        EditorGUILayout.Space(80);
        if(GUILayout.Button("PlayerPrefab", GUILayout.Width(100), GUILayout.Height(20)))
        {
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(PLAYER_FILE_PATH);
            PrefabStageUtility.OpenPrefab(PLAYER_FILE_PATH);
            AnimationWindow animWindow = EditorWindow.GetWindow<AnimationWindow>();
            animWindow.Show();  
            animWindow.Focus();
        }
    }
}

#endif