using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class NewPushableObject : MonoBehaviour
{
    public float unitsPerPush = 1f;
    public float pushSpeed = 5f;
    public float pushDelay = 0.5f;
    
    public LayerMask obstacleLayer;

    private bool isMoving = false;
    private float pushTimer = 0f;
    
    private Rigidbody rb;
    private BoxCollider boxCol;

    public bool isPlayerNear = false, onX, onZ, inPlayed, outPlayed;
    
    private Vector3 safeObjectPosition;
    private Vector3 safePlayerPosition;
    private bool pushCanceled = false;
    
    [HideInInspector] public bool canBePush = true;
    private bool isBlockedByCrowdThisFrame = false;
    
    private void LateUpdate()
    {
        canBePush = !isBlockedByCrowdThisFrame;
        isBlockedByCrowdThisFrame = false;
    }

    public void BlockByCrowd()
    {
        isBlockedByCrowdThisFrame = true;
        canBePush = false;
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        baseSprite = spriteRenderer.sprite;
        rb.isKinematic = true;
        outPlayed = true;
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            
            if (isMoving) return;
            if (!inPlayed)
            {
                inPlayed = true;
                outPlayed = false;
                SoundManager.PlaySound("Interact In");
            }
            
            if (Input.GetKey(KeyCode.Q) || Input.GetButton("Fire1"))
            {
                
                Player.instance.locked = true;
                Player.instance.ChangeState(Player.States.Pushing);
                
                Player.instance.currentPushable = this;
                
                if (Camera.main == null) return;
                
                Vector3 camForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
                
                Vector3 playerDir = (transform.position - Player.instance.transform.position).normalized;

                playerDir = Camera.main.transform.right * playerDir.x + camForward * playerDir.z;
                playerDir = Quaternion.AngleAxis(-45f, Vector3.up) * playerDir;
                
                Player.instance.animatorController.UpdateMoveDirection(playerDir.x, playerDir.z);
                
                Vector3 dir = new Vector3(0f, 0f, 0f);
                if(onX)dir +=(new Vector3(Input.GetAxis("Horizontal"), 0f, 0f));
                if(onZ)dir +=(new Vector3( 0f, 0f,Input.GetAxis("Vertical")));
                pushTimer += Time.deltaTime;

                if (pushTimer >= pushDelay)
                {
                    TryPush(dir.normalized);
                }
            }
            else
            {
                Player.instance.locked = false;
                Player.instance.ChangeState(Player.States.Idle);
                
                Player.instance.currentPushable = null;
                
            }
        }
        else
        {
            spriteRenderer.sprite = baseSprite;
            if (!outPlayed)
            {
                outPlayed = true;
                inPlayed = false;
                SoundManager.PlaySound("Interact Out");
            }
        }
    }

    public SpriteRenderer spriteRenderer;
    private Sprite baseSprite;
    
    [Space]
    
    [Header("Controller Vibration Settings")]
    [Range(0f, 1f), Tooltip("Vibration lourde")]
    public float lowFrequency;
    [Range(0f, 1f), Tooltip("Vibration légere")]
    public float highFrequency;

    private void TryPush(Vector3 direction)
    {
        Vector3 playerToObject = transform.position - Player.instance.transform.position;
        playerToObject.y = 0;
        playerToObject.Normalize();

        float dotProduct = Vector3.Dot(direction, playerToObject);
        bool isPushing = dotProduct > 0.1f; 

        if (isPushing && !canBePush)
        {
            return; 
        }
        
        Vector3 testSize = boxCol.size * 0.45f; 
        Vector3 center = transform.TransformPoint(boxCol.center);

        bool isBlocked = Physics.BoxCast(center, testSize, direction, transform.rotation, unitsPerPush, obstacleLayer);

        if (!isBlocked && direction.magnitude > 0.01f)
        {
            StartCoroutine(Push(direction));
        }
    }

    public void CancelPush()
    {
        if (isMoving && !pushCanceled)
        {
            pushCanceled = true;
        }
    }


    private IEnumerator Push(Vector3 direction)
    { 
        isMoving = true; 
        pushCanceled = false;
        
        safeObjectPosition = rb.position; 
        safePlayerPosition = Player.instance.rb.position;
        
        Vector3 startPos = rb.position; 
        Vector3 targetPos = startPos + (direction * unitsPerPush); 
        Vector3 offset = startPos - Player.instance.transform.position;
        
        Gamepad gamepad = Gamepad.current;
        
        while (Vector3.Distance(rb.position, targetPos) > 0.01f) 
        { 
            if (pushCanceled) 
            { 
                yield return StartCoroutine(RevertPush()); 
                yield break; 
            }
            
            Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, pushSpeed * Time.fixedDeltaTime); 
            rb.MovePosition(newPos); 
            Player.instance.rb.MovePosition(newPos - offset);
            
            if (gamepad != null) 
            { 
                gamepad.SetMotorSpeeds(lowFrequency, highFrequency); 
            }
            
            yield return new WaitForFixedUpdate(); 
        }
    
        rb.MovePosition(targetPos);
        pushTimer = 0f;
        isMoving = false;
        SoundManager.PlaySound("Push", 0.2f);

        if (gamepad != null)
        {
            gamepad.PauseHaptics();
        }
    }
    
    private IEnumerator RevertPush() 
    { 
        Gamepad gamepad = Gamepad.current; 
        Vector3 offset = safeObjectPosition - safePlayerPosition;

    while (Vector3.Distance(rb.position, safeObjectPosition) > 0.01f)
    {
        Vector3 newPos = Vector3.MoveTowards(rb.position, safeObjectPosition, pushSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        Player.instance.rb.MovePosition(newPos - offset);

        yield return new WaitForFixedUpdate();
    }

    rb.MovePosition(safeObjectPosition);
    Player.instance.rb.MovePosition(safePlayerPosition);
    
    pushTimer = 0f;
    isMoving = false;
    pushCanceled = false;

    if (gamepad != null)
    {
        gamepad.PauseHaptics();
    }
    }

}