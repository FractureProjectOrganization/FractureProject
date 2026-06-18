using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPushableHandle : MonoBehaviour
{
    private NewPushableObject obj;
    
    [SerializeField] private GameObject outlineTrigger;

    private OutlineGradient outlineGradient;

    private void Start()
    {
        obj = GetComponentInParent<NewPushableObject>();
        outlineGradient = outlineTrigger.GetComponent<OutlineGradient>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obj.isPlayerNear = true;
            StartCoroutine(HapticManager.instance.InteractionFeedback());
            outlineGradient.FillOutline(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obj.isPlayerNear = false;
            outlineGradient.FillOutline(false);
            obj.ResetPlayer();
        }
    }
}
