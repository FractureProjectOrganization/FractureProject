using System;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance { get; private set; }

    [SerializeField] private Animator cinematicBarsAnimator;

    public bool test;
    
    private static readonly int Cinematic = Animator.StringToHash("isCinematic");
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if (test)
        {
            StartCinematic();
        }
        else
        {
            StopCinematic();
        }
    }

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
}
