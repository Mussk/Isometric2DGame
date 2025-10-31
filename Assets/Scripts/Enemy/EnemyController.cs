using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : BaseEnemyController
    {
       
        [Header("Health")]
        [SerializeField]
        private int currentHealth;

        private Health HealhSystem { get; set; }

        public static event Action OnEnemyDeath;
        
        protected override void Awake()
        {
            base.Awake();
            HealhSystem = new Health(currentHealth, null);

            
        }
        
        private void OnEnable()
        {
            HealhSystem.OnDeath += TriggerDeath;
            //HealhSystem.OnTakingDamage += PlayTakingDamageAnimation;
        
        }

        private void OnDisable()
        {
            HealhSystem.OnDeath -= TriggerDeath;
           // HealhSystem.OnTakingDamage -= PlayTakingDamageAnimation;
        }
        
       
        private async void TriggerDeath()
        {/*
            try
            {
                idleDuration = 10;
                StateMachine.ChangeState(IdleState);
                HealhSystem.IsInvurable = true;
                OnEnemyDeath?.Invoke();
                canMove = false;
                PlayDeathAnimation();
                DisableColliders();
                await Task.Delay(deathDelay);
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }*/
            throw new NotImplementedException();
        }

        
        

        private void PlayDeathAnimation()
        {
           /* animator.SetBool(Move, false);
            animator.SetBool(IsDead, true);
            animator.SetBool(IsAttacking, false);
            animator.SetBool(IsTakingHit, false);
            _cachedPos = transform.position;*/

        }

        private void DisableColliders()
        {
            Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

            foreach (Collider2D collider1 in colliders) 
            { 
                collider1.enabled = false; 
            }
        }
    }
}
