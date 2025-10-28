
using UnityEngine;

public class PlayerController : BasePlayerController
{
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int Move = Animator.StringToHash("CanMove");
    private static readonly int IsTakingHit = Animator.StringToHash("IsTakingHit");
    private static readonly int AnimMoveX = Animator.StringToHash("AnimMoveX");
    private static readonly int AnimMoveY = Animator.StringToHash("AnimMoveY");

    [SerializeField]
    protected int damageAmount;
    public int DamageAmount => damageAmount;
    
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    private SpriteRenderer spriteRenderer;  

    private Vector2 _moveInputCache;

    private Vector2 _animInput;

    
    protected override void Awake()
    {
        base.Awake();
        inputActions.Player.Attack.performed += _ => PerformAttack();
    }

    protected override void Update()
    {
        base.Update();
        AnimateMovement();
    }

    protected void LateUpdate()
    {   
        if(CanMove)
            _moveInputCache = new Vector2(MoveInput.x, MoveInput.y);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Health.OnTakingDamage += PlayOnTakingDamageAnimation;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Health.OnTakingDamage -= PlayOnTakingDamageAnimation;
    }

  /*  protected override void Die()
    {
        base.Die();   
        inputActions.Player.Attack.Disable();
    }*/


    protected override void MovePlayer()
    {
        base.MovePlayer();

        if (!canMove && animator.GetBool(IsAttacking))
        {
            Rb.linearVelocity = Vector2.zero;
            
        } 
    }

    protected void AnimateMovement()
    {
        _animInput = CanMove ? MoveInput : _moveInputCache;

        animator.SetFloat(AnimMoveX, _animInput.x);
        animator.SetFloat(AnimMoveY, _animInput.y);

        if (_animInput.x < 0)
            spriteRenderer.flipX = true;
        else 
            spriteRenderer.flipX = false;

        
    }

    protected void PerformAttack()
    {
        CanMove = false;
        
        PerformAttackAnimation();
        
    }

    protected void PerformAttackAnimation()
    {
        
        animator.SetBool(IsAttacking, true);
        animator.SetBool(Move, false);      
    }


    protected void PlayOnTakingDamageAnimation() 
    {
        //AudioManager.PlaySound(AudioManager.AudioLibrary.TribeSceneSounds.PlayerHit);
        animator.SetBool(IsTakingHit, true);
    
    }

    //Animation end trigger in PlayerAttackAnimationTrigger
}
