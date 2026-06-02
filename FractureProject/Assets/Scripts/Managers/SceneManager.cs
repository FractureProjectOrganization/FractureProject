using System;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    static void Init()
    {
        GameObject obj = new GameObject("SceneManager");
        instance = obj.AddComponent<SceneManager>();
        DontDestroyOnLoad(obj);
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    public void LoadNextScene()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1 > UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            LoadMainMenu();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
