using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : BaseEnemyController
    {
       
        
       // public static event Action OnEnemyDeath;
        
        protected override void Awake()
        {
            base.Awake();
            HealhSystem = new Health(currentHealth, null);

            
        }
        
        private void OnEnable()
        {
            HealhSystem.OnDeath += TriggerDeathState;
            HealhSystem.OnTakingDamage += TriggerHitState;
            EnableColliders();
        }

        private void OnDisable()
        {
            HealhSystem.OnDeath -= TriggerDeathState;
            HealhSystem.OnTakingDamage -= TriggerHitState;
        }
        
       
        private void TriggerDeathState()
        {
                IsDead = true;
                DisableColliders();
                
        }

        private void TriggerHitState()
        => IsGotHit = true;

        //can trigger both trigger and non-trigger collider, used Layer exclusion on collider to fix.
        private void OnTriggerEnter2D(Collider2D other)
        {   
            
            if (other.CompareTag(playerAttackColliderTag))
            {
                HealhSystem.TakeDamage(Player.GetComponent<PlayerController>().DamageAmount);
            }
        }
        
        private void DisableColliders()
        {
            Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

            foreach (Collider2D collider1 in colliders) 
            { 
                collider1.enabled = false; 
            }
        }

        private void EnableColliders()
        {
            Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

            foreach (Collider2D collider1 in colliders) 
            { 
                collider1.enabled = true; 
            }
        }
    }
}
