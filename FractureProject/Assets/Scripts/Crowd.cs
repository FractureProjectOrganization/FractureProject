using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    private CrowdNode rootNode;
    
    private void Awake()
    {
        rootNode = GenerateNodeByChild(gameObject.transform);
        if (debugMode) InitDebugger();
    }
    
    private CrowdNode GenerateNodeByChild(Transform originObject, int nodeIndex = 0)
    {
        if (nodeIndex >= originObject.childCount) return null;
        
        Transform nodeObject = originObject.GetChild(nodeIndex);
        bool isNodeActive = nodeObject.gameObject.activeSelf;
        
        Destroy(nodeObject.gameObject);

        if (nodeObject.childCount > 0)
        {
            CrowdNode[] nextNodes = new CrowdNode[nodeObject.childCount];
            for (int i = 0; i < nodeObject.childCount; i++)
                nextNodes[i] = GenerateNodeByChild(nodeObject.GetChild(i));
            
            return new SwitchCrowdNode(nodeObject.position, nextNodes, isNodeActive);
        }
            
        return new CrowdNode(nodeObject.position, GenerateNodeByChild(originObject, nodeIndex+1), isNodeActive);
    }

    #region DebugManager

    [SerializeField] private bool debugMode;
    private List<CrowdNode> nodes = new List<CrowdNode>();

    private void InitDebugger()
    {
        CrowdNode currentNode = rootNode;
        while (currentNode != null)
        {
            nodes.Add(currentNode);
            currentNode = currentNode.nextNode;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (CrowdNode node in nodes)
        {
            node.DebugNode();
            node.DebugSegment();
        }
    }

    #endregion
}
