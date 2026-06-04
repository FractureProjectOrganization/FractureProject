using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance { get; private set; }
    
    private static readonly int Dezoom = Animator.StringToHash("Dezoom");
 
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject tempText;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject skipText;
    
    public VideoPlayer videoPlayer;
    public UnityEvent OnVideoEndEvent;

    private bool isDezoomed;

    private void Start() 
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        if (isDezoomed) return;
        
        if (Input.anyKey)
        {
            OnVideoEnd(videoPlayer);
        }

    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (isDezoomed) return;
        OnVideoEndEvent.Invoke();
        animator.SetBool(Dezoom, true);
        isDezoomed = true;
        skipText.SetActive(false);
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
