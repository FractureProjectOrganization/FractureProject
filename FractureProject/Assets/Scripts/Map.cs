using System;
using System.Collections;
using UnityEngine;

public class Map : MonoBehaviour
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
    
    [SerializeField] private SpriteRenderer outline;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outline.color = new Color(outline.color.r, outline.color.g, outline.color.b, 1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Fire1")))
        {
            ChangeCam(other);
        }
        else if (other.CompareTag("Player") && !Input.anyKey)
        {
            ResetCam(other);
        }
    }

    private void ChangeCam(Collider other)
    {
        Player.instance.locked = true;
        
        savedTarget = IsometricCameraFollow.instance.GetTarget();

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

    private void ResetCam(Collider other)
    {
        Player.instance.locked = false;
        
        if (isSequence && !sequenceStarted && other.CompareTag("Player"))
        {
            if (savedTarget != null && IsometricCameraFollow.instance != null)
            {
                IsometricCameraFollow.instance.ChangeTarget(savedTarget);
            }
        }

        if (other.CompareTag("Player") && resetOnExit)
        {
            IsometricCameraFollow.instance.ChangeTarget(savedTarget);
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
