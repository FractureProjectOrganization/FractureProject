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

    private bool changedCam;
    private bool playerInside;
    private Collider playerCollider;
    
    [SerializeField] private GameObject outlineTrigger;

    private OutlineGradient outlineGradient;

    private void Start()
    {
        outlineGradient = outlineTrigger.GetComponent<OutlineGradient>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerCollider = other;
            outlineGradient.FillOutline(true);
            StartCoroutine(HapticManager.instance.InteractionFeedback());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerCollider = null;
            outlineGradient.FillOutline(false);
        }
    }

    private void Update()
    {
        if (!playerInside || !playerCollider) return;

        if (InputManager.Instance.Interact.WasPressedThisFrame())
        {
            if (!changedCam)
            {
                ChangeCam(playerCollider);
            }
            else if (changedCam)
            {
                ResetCam(playerCollider);
            }
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
        
        changedCam = true;
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
        
        changedCam = false;
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
