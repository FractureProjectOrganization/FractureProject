using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class NodeStateTrigger : MonoBehaviour, INodeStateListener
{
    [SerializeField] private UnityEvent action;

    [SerializeField] private CrowdState targetState;

    [SerializeField] private bool oneTimeOnly;

    private bool isReady = false;
    
    private CrowdNode node;

    private void Start()
    {
        StartCoroutine(WaitingPatchForReady());
    }

    public void ListenNode(CrowdNode node)
    {
        this.node = node;
    }

    public void OnStateChange()
    {
        if (!isReady) return;

        if (node.state == targetState)
        {
            action.Invoke();
            if (oneTimeOnly)
                node.DisconnectListener();
        }

        
    }

    private IEnumerator WaitingPatchForReady()
    {
        yield return new WaitForSeconds(2f);
        isReady = true;
    }
}
