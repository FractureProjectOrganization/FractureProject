using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance { get; private set; }
    
    private static readonly int TrOuvertureMenuPause = Animator.StringToHash("Tr_OuvertureMenuPause");
    private static readonly int TrFermetureMenuPause = Animator.StringToHash("Tr_FermetureMenuPause");
    private static readonly int TrOuvertureSetting = Animator.StringToHash("Tr_OuvertureSetting");
    private static readonly int TrFermetureSetting = Animator.StringToHash("Tr_FermetureSetting");

    [SerializeField] private GameObject pauseMenuPanel;
    
    private InputAction pauseAction, apanyanAction;
    private Animator animator;

    private bool isPaused;
    public bool isMainMenu;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        pauseAction = GetComponent<PlayerInput>().actions["Pause"];
        apanyanAction = GetComponent<PlayerInput>().actions["Crouch"];

        pauseAction.actionMap.Enable();
        apanyanAction.actionMap.Enable();
    }

    private void OnEnable()
    {
        pauseAction.performed += OnPauseButtonPress;
        apanyanAction.performed += OnApanyanAction;
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPauseButtonPress;
        apanyanAction.performed -= OnApanyanAction;
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
    
    private void OnApanyanAction(InputAction.CallbackContext context)
    {
        if (!pauseMenuPanel) return;

        if (isPaused)
        {
            
        }
        else if (!isPaused)
        {
            CloseSettings();
        }
    }
    
    private void OnPause()
    {
        if (!pauseMenuPanel) return;
        
        //Time.timeScale = 0;
        if(Player.instance)Player.instance.locked = true;
        UIManager.instance.SetPauseButton();
        
        animator.SetTrigger(TrOuvertureMenuPause);

        isPaused = true;
    }

    public void Resume()
    {
        if (!pauseMenuPanel) return;
        
        //Time.timeScale = 1;
        UIManager.instance.RemoveFirstSelectedButton();
        animator.SetTrigger(TrFermetureMenuPause);
        
        if(Player.instance)Player.instance.locked = false;

        isPaused = false;
    }
    
    

    public void OpenSettings()
    {
        if (!pauseMenuPanel) return;
        if(!isMainMenu)animator.SetTrigger(TrOuvertureSetting);
        else animator.SetTrigger("Tr_OuvSettingMainMenu");
        
        StartCoroutine(WaitForSettings());
    }
    
    public void CloseSettings()
    {
        if (!pauseMenuPanel) return;
        if(!isMainMenu)animator.SetTrigger(TrFermetureSetting);
        else
        {
            animator.SetTrigger("Tr_FerSettingMainMenu");
            UIManager.instance.SetMainMenuButton();
        }

        
    }

    private IEnumerator WaitForSettings()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        UIManager.instance.SetSettingsButton();

    }


    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
