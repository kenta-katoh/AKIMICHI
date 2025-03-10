using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlaySceneButton
{
    [MenuItem("Akimichi/PlayGame")]
    /// <summary>
    /// Scene実行処理
    /// </summary>
    static void PlayScene()
    {
        string scenePath = EditorBuildSettings.scenes[0].path;
        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

        if (sceneAsset == null)
        {
            Debug.Log($"{scenePath} シーンアセットが存在しません");
            return;
        }

        EditorSceneManager.playModeStartScene = sceneAsset;

        EditorApplication.isPlaying = true;
    }
}
