using System;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    public bool isVf { get; private set; }

    public Action OnLanguageChange;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeLanguage(bool vfState)
    {
        isVf = vfState;
        OnLanguageChange?.Invoke();
    }
    
}
