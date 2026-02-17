using Unity.VisualScripting;
using UnityEngine;

public class CrowdNode
{
    public CrowdNode nextNode;
    
    public Vector3 position;
    public bool isActive;
    
    public CrowdNode(Vector3 position, CrowdNode nextNode, bool isActive = false)
    {
        this.position = position;
        this.isActive = isActive;
        this.nextNode = nextNode;
    }

    public void CheckObstacles()
    {
        
    }

    private void Split()
    {
        
    }

    private void Merge()
    {
        
    }

    public virtual void DebugNode()
    {
        Gizmos.color = isActive ? Color.blue : Color.gray;
        Gizmos.DrawSphere(position, 0.2f);
    }

    public virtual void DebugSegment()
    {
        if (nextNode == null) return;
        Gizmos.color = nextNode.isActive ? Color.blue : Color.gray;
        Gizmos.DrawLine(position, nextNode.position);
    }
    
}

public class DynamicCrowdNode : CrowdNode
{
    public DynamicCrowdNode(Vector3 position, CrowdNode nextNode, bool isActive = false) : base(position, nextNode, isActive)
    {
    }

    public override void DebugNode()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position, 0.2f);
    }

    public void UpdatePosition()
    {
        
    }
}

public class SwitchCrowdNode : CrowdNode
{
    public CrowdNode[] nextNodes;
    
    public SwitchCrowdNode(Vector3 position, CrowdNode[] nextNodes, bool isActive = false) : base(position, null, isActive)
    {
        
    }

    public override void DebugNode()
    {
        Gizmos.color = Color.darkViolet;
        Gizmos.DrawSphere(position, 0.2f);
    }
    
    public override void DebugSegment()
    {
        foreach (CrowdNode node in nextNodes)
        {
            Gizmos.color = node.isActive ? Color.darkViolet : Color.violet;
            Gizmos.DrawLine(position, node.position);
        }
    }
    
    public void Switch()
    {
        
    }
}