using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;

    private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
    private static readonly int IsTransportedHash = Animator.StringToHash("isTransported");
    private static readonly int IsPushingHash = Animator.StringToHash("isPushing");//Nico
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int IsDown = Animator.StringToHash("isDown"); //Stoian
    private static readonly int FightFactor = Animator.StringToHash("FightFactor"); //Stoian
    
    public bool isForFighting;

    public void OnStateChanged(Player.States newState)
    {
        if (isForFighting && newState == Player.States.Hit)
        {
            animator.SetTrigger(Hit);
        }

        else if (isForFighting && newState == Player.States.Down)
        {
            animator.SetTrigger(IsDown);
        }
        
        else if (isForFighting && newState == Player.States.Attacking)
        {
            animator.SetTrigger(Attack);
        }
        
        animator.SetBool(IsMovingHash, newState == Player.States.Walking);
        animator.SetBool(IsTransportedHash, newState == Player.States.Transported);
        animator.SetBool(IsPushingHash,newState == Player.States.Pushing);//Nico
    }
    
    public void SetInCombat(bool state)
    {
        animator.SetFloat(FightFactor, state ? 1f : 0f);
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
