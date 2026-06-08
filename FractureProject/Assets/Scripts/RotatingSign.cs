using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RotatingSign : MonoBehaviour
{
    [SerializeField] private GameObject outlineTrigger;

    private OutlineGradient outlineGradient;
    private RotatingPanneau visuel;
    
    [Space]

    public UnityEvent onInteraction;

    private bool isPlayerNear, cooldown;
    private void Start()
    {
        visuel = GetComponent<RotatingPanneau>();
        outlineGradient = outlineTrigger.GetComponent<OutlineGradient>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            outlineGradient.FillOutline(true);
            
            StartCoroutine(HapticManager.instance.InteractionFeedback());
            SoundManager.PlaySound("Interact In");
        }
    }
    

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
            outlineGradient.FillOutline(false);
            
            SoundManager.PlaySound("Interact Out");
        }

    }

    private void Update()
    {
        if (isPlayerNear)
        {
            if (NewInput.GetInteractDown())
            {
                if (cooldown) return;
                cooldown = true;
                StartCoroutine(Cooldown());
                onInteraction.Invoke();
                visuel.Turn();
            }
        }
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        {
            cooldown = false;
        }
    }
}
