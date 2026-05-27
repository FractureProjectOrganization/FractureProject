using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public KeyCode toggleKey = KeyCode.F2;
    public bool showMenu = true;
    
    // [Header("Player")]
    // public Transform player;
    // public int coinsCollected = 12;
    // public int enemiesDefeated = 4;
    //
    // [Header("Performance")]
    // public bool showFPS = true;
    //
    // [Header("Session")]
    // public float sessionStartTime;

    private float deltaTime;

    private Vector2 scrollPosition;

    void Start()
    {
        // sessionStartTime = Time.time;
        showMenu = !showMenu;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
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
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(i);
            }
        }

        GUILayout.EndScrollView();

        GUILayout.Space(10);

        GUILayout.Label(toggleKey + " to toggle menu");

        GUILayout.EndArea();
    }
}