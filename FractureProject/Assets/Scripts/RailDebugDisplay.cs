using UnityEngine;

public class RailDebugDisplay : MonoBehaviour
{
    private Color railColor = Color.blue;
    private Color exitColor = Color.red;
    private float nodeSize = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = railColor;

        for (int i = 1; i < transform.childCount; i++)
        {
            Transform currentPoint = transform.GetChild(i);
            Transform previousPoint = transform.GetChild(i - 1);
            
            Gizmos.DrawSphere(previousPoint.position, nodeSize);
            
            Gizmos.DrawLine(previousPoint.position, currentPoint.position);
        }
        
        Gizmos.color = exitColor;
        Transform exitPoint = transform.GetChild(transform.childCount-1); 
        Gizmos.DrawSphere(exitPoint.position, nodeSize);
    }
}
