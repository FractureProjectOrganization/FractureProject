using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance { get; private set; }
 
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject tempText;
    
    private InputAction pauseAction;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void StartGame()
    {
        if (!mainMenuPanel) return;
        
        TransitionManager.instance.FadeToBlack();
    }

    public void Settings()
    {
        //TODO: Settings Menu
        
        if (!mainMenuPanel) return;
        
        tempText.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
