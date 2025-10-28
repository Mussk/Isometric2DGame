using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimationTrigger : MonoBehaviour
{
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int CanMove = Animator.StringToHash("CanMove");
    private static readonly int IsTakingHit = Animator.StringToHash("IsTakingHit");

    [SerializeField]
    private PlayerController controller;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private List<GameObject> colliders;

    [SerializeField]
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        DisableAllColliders();
    }

    //this method is triggerd by animation
    public void OnAttackAnimationEnd()
    {
        controller.CanMove = true;
        animator.SetBool(IsAttacking, false);
        animator.SetBool(CanMove, true);
        controller.UpdateMoveInput();
    }

    //this method is triggerd by animation
    public void AttackAnimationColliderTrigger(string direction)
    {
        DisableAllColliders();

        direction = direction.ToLower();

        foreach (GameObject go in colliders)
        {
            string name = go.name.ToLower();
            Collider2D col = go.GetComponent<Collider2D>();
            bool flipped = spriteRenderer.flipX;

            if (name.Contains(direction))
            {
                if (direction == "right")
                {
                    if (!flipped && name.Contains(direction))
                    {
                        col.enabled = true;
                    }
                    continue;
                }
                col.enabled = true;
            }
            else
            {
                if (flipped && name.Contains("left"))
                    col.enabled = true;
            }
            
        }
    }


    //this method is triggerd by animation
    public void DisableAttackColliderTrigger()
    {
        //AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.PlayerAttack);
        DisableAllColliders();
    }


    private void DisableAllColliders()
    {
        colliders.ForEach(c => c.GetComponent<Collider2D>().enabled = false);
    }

    private void OnTakingHitAnimationEnd()
    {
        animator.SetBool(IsTakingHit, false);
    }
}
