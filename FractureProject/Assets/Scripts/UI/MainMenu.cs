using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance { get; private set; }
    
    private static readonly int Dezoom = Animator.StringToHash("Dezoom");
 
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject tempText;
    [SerializeField] private Animator animator;
    
    public VideoPlayer videoPlayer;

    void Start() 
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp) 
    {
        animator.SetTrigger(Dezoom);
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
