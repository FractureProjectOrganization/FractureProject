using UnityEngine;

public class CameraTargetTrigger : MonoBehaviour
{
    public Transform cameraTargetPoint;
    public bool resetOnExit = false;
    private Transform savedTarget;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (resetOnExit) savedTarget = IsometricCameraFollow.instance.GetTarget();
            IsometricCameraFollow.instance.ChangeTarget(cameraTargetPoint);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(resetOnExit) IsometricCameraFollow.instance.ChangeTarget(savedTarget);
        }
    }
    
}
