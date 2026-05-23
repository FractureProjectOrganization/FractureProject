using System.Collections;
using UnityEngine;

public class CameraTargetTrigger : MonoBehaviour
{
    public Transform cameraTargetPoint;
    public bool resetOnExit = false, isSequence = false;
    private Transform savedTarget;

    [Header("Paramètres de séquence")] [Tooltip("Doit être cochée pour utiliser les paramètres ci-dessous")]
    
    private bool sequenceStarted = false;
    
    [Tooltip("Permet de déclencher un mouvement de caméra vers une target")]
    public bool startSequence;
    
    [Tooltip("Temps avant le reset de la caméra")]
    public float observationTime = 0f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            savedTarget = other.transform;

            if (IsometricCameraFollow.instance != null)
            {
                IsometricCameraFollow.instance.ChangeTarget(cameraTargetPoint);
            }
            
            if (!sequenceStarted)
            {
                if (startSequence)
                {
                    sequenceStarted = true;
                    StartCoroutine(BlockAndReleasePlayer(other.gameObject));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isSequence && !sequenceStarted && other.CompareTag("Player"))
        {
            if (savedTarget != null && IsometricCameraFollow.instance != null)
            {
                IsometricCameraFollow.instance.ChangeTarget(savedTarget);
            }
        }
    }
    
    private IEnumerator BlockAndReleasePlayer(GameObject playerObject)
    {
        Player activePlayer = playerObject.GetComponent<Player>();
        if (activePlayer == null)
        {
            activePlayer = playerObject.GetComponentInParent<Player>();
        }

        if (activePlayer != null)
        {
            activePlayer.LockPlayer(true);
        }
        
        yield return new WaitForSeconds(observationTime);

        if (activePlayer != null)
        {
            activePlayer.LockPlayer(false);
        }
        else
        {
            Debug.Log("Player not found");
        }
        
        if (savedTarget != null)
        {
            IsometricCameraFollow.instance.ChangeTarget(savedTarget);
        }
        
        sequenceStarted = false;
        
        FlashbackGuards guardsZone = GetComponentInParent<FlashbackGuards>();
        if (guardsZone != null)
        {
            guardsZone.MarkAsObserved();
        }
    }
}