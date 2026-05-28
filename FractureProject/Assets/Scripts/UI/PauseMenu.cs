using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance { get; private set; }
    
    private static readonly int TrOuvertureMenuPause = Animator.StringToHash("Tr_OuvertureMenuPause");
    private static readonly int TrFermetureMenuPause = Animator.StringToHash("Tr_FermetureMenuPause");
    private static readonly int TrOuvertureSetting = Animator.StringToHash("Tr_OuvertureSetting");
    private static readonly int TrFermetureSetting = Animator.StringToHash("Tr_FermetureSetting");

    [SerializeField] private GameObject pauseMenuPanel;
    
    private InputAction pauseAction;
    private Animator animator;

    private bool isPaused;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        pauseAction = GetComponent<PlayerInput>().actions["Pause"];
        pauseAction.actionMap.Enable();
    }

    private void OnEnable()
    {
        pauseAction.performed += OnPauseButtonPress;
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPauseButtonPress;
    }
    
    private void Start()
    {
        if (!pauseMenuPanel) return;

        animator = pauseMenuPanel.GetComponent<Animator>();
        foreach (Animator anim in pauseMenuPanel.GetComponentsInChildren<Animator>(true))
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    private void OnPauseButtonPress(InputAction.CallbackContext context)
    {
        if (!pauseMenuPanel) return;
        
        if (isPaused)
        {
            Resume();
        }
        else if (!isPaused)
        {
            OnPause();
        }
    }
    
    private void OnPause()
    {
        if (!pauseMenuPanel) return;
        
        Time.timeScale = 0;
        Player.instance.locked = true;
        UIManager.instance.SetPauseButton();
        
        animator.SetTrigger(TrOuvertureMenuPause);

        isPaused = true;
    }

    public void Resume()
    {
        if (!pauseMenuPanel) return;
        
        Time.timeScale = 1;
        UIManager.instance.RemoveFirstSelectedButton();
        animator.SetTrigger(TrFermetureMenuPause);
        
        Player.instance.locked = false;

        isPaused = false;
    }

    public void OpenSettings()
    {
        if (!pauseMenuPanel) return;
        
        animator.SetTrigger(TrOuvertureSetting);
    }

    public void CloseSettings()
    {
        if (!pauseMenuPanel) return;
        
        animator.SetTrigger(TrFermetureSetting);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
