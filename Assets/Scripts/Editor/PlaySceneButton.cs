using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Linq;
using Akimichi;

public class PlaySceneButton : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Akimichi/PlayGame")]
    /// <summary>
    /// Scene実行処理
    /// </summary>
    static void PlayScene()
    {
        var window = GetWindow<PlaySceneButton>("UIElements");
        window.titleContent = new GUIContent("PlayScene");
        window.Show();
    }

    static string[] listScenes = { SceneConst.Title, SceneConst.Home, SceneConst.Tutorial, SceneConst.Result };

    private void OnGUI()
    {
        GUILayoutOption[] buttonOption = new GUILayoutOption[]
        {
            GUILayout.Height(30)
        };

        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        foreach (var scene in EditorBuildSettings.scenes)
        {
            string path = scene.path;
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            
            if (!listScenes.Contains(sceneName)) continue;
            if (GUILayout.Button(sceneName, buttonOption))
            {
                if(!EditorApplication.isPlaying)
                {
                    SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                    EditorSceneManager.playModeStartScene = sceneAsset;
                    EditorApplication.isPlaying = true;
                }
            }
        }
    }
#endif
}
