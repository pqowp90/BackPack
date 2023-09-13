using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

public class AudioAssetRenamer
{
    [MenuItem("Custom/RenameDuplicateAudioClips")]
    static void RenameDuplicateAudioClips()
    {
        // Get all audio clips in the project
        string[] audioClipGUIDs = AssetDatabase.FindAssets("t:AudioClip");
        
        // Store the names and occurrences of audio clips
        var audioClipNames = new Dictionary<string, int>();

        // Iterate through each audio clip
        foreach (var guid in audioClipGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

            if (audioClipNames.ContainsKey(assetName))
            {
                // Duplicate name found, increment the occurrence count
                audioClipNames[assetName]++;
                string newAssetName = $"{assetName}_{audioClipNames[assetName]}";
                string newAssetPath = $"{System.IO.Path.GetDirectoryName(assetPath)}/{newAssetName}.asset";

                // Rename the asset
                AssetDatabase.RenameAsset(assetPath, newAssetName);
                AssetDatabase.MoveAsset(assetPath, newAssetPath);
            }
            else
            {
                // First occurrence of the name, add it to the dictionary
                audioClipNames.Add(assetName, 0);
            }
        }

        // Refresh the Asset Database to reflect the changes
        AssetDatabase.Refresh();
    }
}
#endif