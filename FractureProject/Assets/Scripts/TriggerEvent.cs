using System;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerEnterAction, /*nico*/ onTriggerExitAction;
    
    [SerializeField] private string targetTag;
    
    public bool isTalking;
    public bool initDialog;
    public bool once = false;

    private bool doneOnce = false;
    
    public Dialogs dialogs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            if (once && doneOnce) return;
            if (once) doneOnce = true;
            onTriggerEnterAction?.Invoke();
            isTalking = true;
            
            if (!initDialog && dialogs !=null)
            {
                initDialog = true;
                DialogManager.instance.InitiateDialogue(dialogs);
                Debug.Log("dialog initiated");
            }
            
            if (DialogManager.instance != null) 
                DialogManager.instance.StartDialogue(dialogs);
        }
    }

        
    
    //Nico
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            onTriggerExitAction?.Invoke();
            isTalking = false;
        }
    }
    
    //Stoian
    public void DeleteTrigger()
    {
        Destroy(this);
    }
}