using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance { get; private set; }

    [SerializeField] private Animator cinematicBarsAnimator;
    [SerializeField] private Animator fadeToBlackAnimator;
    
    private static readonly int Cinematic = Animator.StringToHash("isCinematic");
    private static readonly int Black = Animator.StringToHash("isBlack");

    public float TimeBforeSceneSwitch = 0f;

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
        
        fadeToBlackAnimator.SetBool(Black, true);
        StartCoroutine(WaitForFade());
    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(2.5f);
        yield return new WaitForSeconds(TimeBforeSceneSwitch);
        SceneManager.instance.LoadNextScene();
        yield return null;
    }

    public void FadeFromBlack()
    {
        if (!fadeToBlackAnimator) return;
        
        fadeToBlackAnimator.SetBool(Black, false);
        Player.instance.locked = false;
    }
    #endregion
}
