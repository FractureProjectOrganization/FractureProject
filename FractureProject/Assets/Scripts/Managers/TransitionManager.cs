using System;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance { get; private set; }

    [SerializeField] private Animator cinematicBarsAnimator;
    [SerializeField] private Animator fadeToBlackAnimator;
    
    private static readonly int Cinematic = Animator.StringToHash("isCinematic");
    private static readonly int Black = Animator.StringToHash("isBlack");

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    #region Cinematic Bars
    public void StartCinematic()
    {
        if (!cinematicBarsAnimator) return;
        
        cinematicBarsAnimator.SetBool(Cinematic, true);
        
        //Trigger Camera movement (slight zoom-in by default)
    }

    public void StopCinematic()
    {
        if (!cinematicBarsAnimator) return;
        
        cinematicBarsAnimator.SetBool(Cinematic, false);
        
        //Trigger Camera movement (slight zoom-out by default)
    }
    #endregion

    #region FadeToBlack
    public void FadeToBlack()
    {
        if (!fadeToBlackAnimator) return;

        Player.instance.locked = true;
        fadeToBlackAnimator.SetBool(Black, true);
    }

    public void FadeFromBlack()
    {
        if (!fadeToBlackAnimator) return;
        
        fadeToBlackAnimator.SetBool(Black, false);
        Player.instance.locked = false;
    }
    #endregion
}
