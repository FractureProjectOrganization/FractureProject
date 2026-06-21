using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance { get; private set; }
    
    private static readonly int TrOuvertureMenuPause = Animator.StringToHash("Tr_OuvertureMenuPause");
    private static readonly int TrFermetureMenuPause = Animator.StringToHash("Tr_FermetureMenuPause");
    private static readonly int TrOuvertureSetting = Animator.StringToHash("Tr_OuvertureSetting");
    private static readonly int TrFermetureSetting = Animator.StringToHash("Tr_FermetureSetting");

    [SerializeField] private GameObject pauseMenuPanel;
    
    private Animator animator;

    private bool isPaused, isSettingOpen;
    public bool isMainMenu;

    public bool isBlocked = false;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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

    private void Update()
    {
        if (NewInput.GetPauseDown())
        {
            if (isMainMenu) return;
            if (!(Player.instance.currentState == Player.States.Idle || Player.instance.currentState == Player.States.Walking)) return;
            if (isBlocked) return;
            
            HandlePauseInput();
        }

        if (NewInput.GetBackDown()) 
        {
            if (isMainMenu && !isSettingOpen) return;
            HandleApanyanInput();
        }
    }

    private void HandlePauseInput()
    {
        if (!pauseMenuPanel) return;
        
        if (isPaused)
        {
            if(isSettingOpen) CloseSettings();
            else Resume();
        }
        else
        {
            OnPause();
        }
    }
    
    private void HandleApanyanInput()
    {
        if (!pauseMenuPanel) return;

        if (isMainMenu)
        {
            CloseSettings();
            return;
        }
        if (isPaused && isSettingOpen)
        {
            CloseSettings();
        }
        else if (isPaused && !isSettingOpen)
        {
            Resume();
        }
    }
    
    private void OnPause()
    {
        if (!pauseMenuPanel) return;
        
        if(Player.instance) Player.instance.locked = true;
        UIManager.instance.SetPauseButton();
        
        animator.SetTrigger(TrOuvertureMenuPause);

        isPaused = true;
    }

    public void Resume()
    {
        if (!pauseMenuPanel) return;
        
        isPaused = false;
        UIManager.instance.RemoveFirstSelectedButton();
        animator.SetTrigger(TrFermetureMenuPause);
        
        if(Player.instance) Player.instance.locked = false;
    }
    
    public void OpenSettings()
    {
        if (!pauseMenuPanel) return;
        if(!isMainMenu) animator.SetTrigger(TrOuvertureSetting);
        else animator.SetTrigger("Tr_OuvSettingMainMenu");
        
        isSettingOpen = true;
        
        StartCoroutine(WaitForSettings());
    }
    
    public void CloseSettings()
    {
        if (!pauseMenuPanel) return;
        if (!isMainMenu)
        {
            animator.SetTrigger(TrFermetureSetting);
            UIManager.instance.SetPauseButton();
        }
        else
        {
            animator.SetTrigger("Tr_FerSettingMainMenu");
            UIManager.instance.SetMainMenuButton();
        }
        isSettingOpen = false;
    }

    private IEnumerator WaitForSettings()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if(UIManager.instance)UIManager.instance.SetSettingsButton();
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}