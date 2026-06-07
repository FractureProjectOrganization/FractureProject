using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public Key toggleKey = Key.F2;
    public bool showMenu = true;

    private float deltaTime;
    private Vector2 scrollPosition;

    void Start()
    {
        showMenu = !showMenu;
    }
    
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            showMenu = !showMenu;
        }
        
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (!showMenu) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 260), "Debug Scene Menu", GUI.skin.window);
        GUILayout.Label("Available Scenes:");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (GUILayout.Button(sceneName, GUILayout.Height(30)))
                UnityEngine.SceneManagement.SceneManager.LoadScene(i);
        }

        GUILayout.EndScrollView();
        GUILayout.Space(10);
        GUILayout.Label(toggleKey + " to toggle menu");
        GUILayout.EndArea();
    }
}