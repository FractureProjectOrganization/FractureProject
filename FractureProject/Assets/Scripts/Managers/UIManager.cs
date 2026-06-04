using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private GameObject firstPauseButton;
    [SerializeField] private GameObject firstSettingsButton;
    [SerializeField] private GameObject firstMainMenuButton;


    public static UIManager instance { get; private set; } = null;
    
    public bool isMainMenu = false;
    
    public bool DestroyOnLoad = false;

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    static void Init()
    {
        
        //GameObject obj = new GameObject("UIManager");
        //instance = obj.AddComponent<UIManager>();
        //DontDestroyOnLoad(obj);
    }*/
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning(instance.name);
            Destroy(gameObject);
            return;
        }
        instance = this;
        if(!DestroyOnLoad) DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if(isMainMenu) EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }

    public void SetPauseButton()
    {
        if (!firstPauseButton) return;
        
        EventSystem.current.SetSelectedGameObject(firstPauseButton);
    }

    public void SetSettingsButton()
    {
        if (!firstSettingsButton) return;
        
        EventSystem.current.SetSelectedGameObject(firstSettingsButton);
    }
    
    public void SetMainMenuButton()
    {
        if (!firstMainMenuButton) return;
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }

    public void RemoveFirstSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
