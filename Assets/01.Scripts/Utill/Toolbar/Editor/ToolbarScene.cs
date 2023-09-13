#if UNITY_EDITOR
using System.Linq;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using UnityToolbarExtender;
using System.IO;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class ToolbarScene
{
    private const string SCENES_FILE_PATH = "/03.Scenes";
    static ToolbarScene()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    private static void OnToolbarGUI(IMGUIEvent evt)
    {
        Debug.Log($"OnToolbarGUI {evt.target}");
    }

    static void OnToolbarGUI()
    {
        GUIContent content = new GUIContent(EditorSceneManager.GetActiveScene().name);

        string filePath = $"{Application.dataPath}{SCENES_FILE_PATH}";//Path.Combine(Application.dataPath, SCENES_FILE_PATH);

        // Debug.Log(Path.Combine(Application.dataPath, SCENES_FILE_PATH));
        Rect dropdownRect = new Rect(5, 0, 150, 20);
        if (EditorGUI.DropdownButton(dropdownRect, content, FocusType.Keyboard, EditorStyles.toolbarDropDown))
        {
            GenericMenu menu = new GenericMenu();
            MakeSceneMenus(filePath, menu);
            menu.DropDown(dropdownRect);
        }
    }

    private static void MakeSceneMenus(string path, GenericMenu menu, string addPath = "")
    {
        string[] scenes = {};
        try
        {
            scenes = Directory.GetFileSystemEntries(path);
        }
        catch{}

        foreach (var scene in scenes)
        {
            int dotIndex = scene.LastIndexOf('.');
            if(dotIndex == -1) continue;
            
            string substring = scene.Substring(dotIndex);

            if(substring == ".meta") continue;
            
            string extension = Path.GetFileNameWithoutExtension(scene);
            int extenDotIndex = extension.LastIndexOf('.');
            if (substring == ".unity")
            {
                int assetsIndex = scene.IndexOf("Assets");

                if (assetsIndex == -1) continue;

                menu.AddItem(new GUIContent($"{addPath}{extension}"), false, () =>
                {
                    if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()){
                        EditorSceneManager.OpenScene(scene.Substring(assetsIndex));
                    }
                });
            }
            else
            {
                if (addPath == "")
                {
                    MakeSceneMenus(scene, menu, extension + "/");
                }
                else
                {
                    MakeSceneMenus(scene, menu, addPath + extension + "/");
                }
            }
        }
    }

}
#endif