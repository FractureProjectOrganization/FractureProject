using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private GameObject firstPauseButton;
    [SerializeField] private GameObject firstSettingsButton;

    public static UIManager instance { get; private set; } = null;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
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

    public void RemoveFirstSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
