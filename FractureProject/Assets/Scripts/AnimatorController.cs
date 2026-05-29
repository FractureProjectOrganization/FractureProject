using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;

    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    private static readonly int IsTransportedHash = Animator.StringToHash("isTransported");
    private static readonly int IsPushingHash = Animator.StringToHash("isPushing");//Nico
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int IsDown = Animator.StringToHash("isDown"); //Stoian
    private static readonly int InCombat = Animator.StringToHash("inCombat"); //Stoian
    
    private bool hasIsAttackingParam;
    
    void Start()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.nameHash == IsAttacking)
            {
                hasIsAttackingParam = true;
                break;
            }
        }
    }

    public void OnStateChanged(Player.States newState)
    {
        if (hasIsAttackingParam && newState == Player.States.Hit)
        {
            animator.SetTrigger(Hit);
        }

        if (hasIsAttackingParam && newState == Player.States.Down)
        {
            animator.SetTrigger(IsDown);
        }
        
        animator.SetBool(IsMovingHash, newState == Player.States.Walking);
        animator.SetBool(IsTransportedHash, newState == Player.States.Transported);
        animator.SetBool(IsPushingHash,newState == Player.States.Pushing);//Nico
        
        if (hasIsAttackingParam)
        {
            animator.SetBool(IsAttacking, newState == Player.States.Attacking);
        }
    }
    
    public void SetInCombat(bool value)
    {
        animator.SetBool(InCombat, value);
    }

    public void UpdateMoveDirection(float dirX, float dirY)
    {
        if (Mathf.Abs(dirX) > 0.01f)
        {
            float snappedX = dirX > 0 ? 1f : -1f;
            animator.SetFloat(MoveXHash, snappedX);
        }

        if (Mathf.Abs(dirY) > 0.01f)
        {
            float snappedY = dirY > 0 ? 1f : -1f;
            animator.SetFloat(MoveYHash, snappedY);
        }
    }
}
