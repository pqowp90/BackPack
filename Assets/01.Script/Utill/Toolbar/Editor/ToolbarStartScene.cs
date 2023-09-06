#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class ToolbarStartScene
{
    static ToolbarStartScene()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

        OnValueChanged(EditorPrefs.GetInt("StartInMainMenu", 0) == 1);
    }

    private static void OnToolbarGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Start in Main Menu", GUILayout.Width(110));
        var value = EditorPrefs.GetInt("StartInMainMenu", 0);
        var startInMainMenu = EditorGUILayout.Toggle(value == 1);
        EditorGUILayout.EndHorizontal();

        switch (value)
        {
            case 1 when !startInMainMenu:
                OnValueChanged(false);
                break;
            case 0 when startInMainMenu:
                OnValueChanged(true);
                break;
        }
    }

    private static void OnValueChanged(bool value)
    {
        EditorPrefs.SetInt("StartInMainMenu", value ? 1 : 0);

        EditorSceneManager.playModeStartScene =
            value ? AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/03.Scenes/MainMenu.unity") : null;
    }
}
#endif