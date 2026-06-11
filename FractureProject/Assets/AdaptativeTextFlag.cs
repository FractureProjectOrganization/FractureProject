using System;
using TMPro;
using UnityEngine;

public class AdaptativeTextFlag : MonoBehaviour
{
    [SerializeField] private string textVo;
    [SerializeField] private string textVf;
    
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        LanguageManager.instance.OnLanguageChange += UpdateDisplay;
        UpdateDisplay();
    }

    private void OnDestroy()
    {
        LanguageManager.instance.OnLanguageChange -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        text.text = LanguageManager.instance.isVf ? textVf : textVo;
    }
}
