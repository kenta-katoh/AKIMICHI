using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneManager : MonoBehaviour
{
    private static TestSceneManager instance = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private TestSceneManager()
    {
    }

    public static TestSceneManager Instance()
    {
        if (instance == null) instance = new TestSceneManager();
        return instance;
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    /// <param name="name"></param>
    public void ChangeScene(string name)
    {
        NetworkManager.Instance().DeleteCallBack();
        SceneManager.LoadScene(name);
    }
}
