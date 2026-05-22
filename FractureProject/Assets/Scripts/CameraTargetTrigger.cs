using System.Collections.Generic;
using UnityEngine;

public class CameraTargetTrigger : MonoBehaviour
{
    public Transform cameraTargetPoint;
    public bool resetOnExit = false;
    private Transform savedTarget;

    [Header("Paramètres de séquence")] [Tooltip("Temps avant que la caméra revienne seule")]
    public float observationTime = 1f;
    private bool sequenceStarted = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (resetOnExit) savedTarget = IsometricCameraFollow.instance.GetTarget();
            IsometricCameraFollow.instance.ChangeTarget(cameraTargetPoint);

            if (!sequenceStarted)
            {
                sequenceStarted = true;
                StartCoroutine(BlockAndReleasePlayer(other.gameObject));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(resetOnExit) IsometricCameraFollow.instance.ChangeTarget(savedTarget);
        }
    }
    
    //Fonction spécifique pour adapter le script aux gardes dans les flashbacks
    private IEnumerator<> BlockAndReleasePlayer(GameObject player)
    {
        if (Player.instance != null) Player.instance.LockPlayer(true);
        yield return new WaitForSeconds(observationTime);

        if (resetOnExit && savedTarget != null)
        {
            IsometricCameraFollow.instance.ChangeTarget(savedTarget);
        }
        
        if (Player.instance != null) Player.instance.LockPlayer(false);
    }
}
