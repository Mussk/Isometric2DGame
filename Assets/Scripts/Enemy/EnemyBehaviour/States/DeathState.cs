using System;
using Enemy.EnemyBehaviour.States;
using UnityEngine;
using UnityHFSM;

namespace Enemy.EnemyBehaviour.States
{
    public class DeathState : EnemyStateBase
    {
        protected BaseEnemyController BaseEnemyController;
        protected float exitTime;
        public DeathState(bool needsExitTime,
            BaseEnemyController enemy,
            float exitTime = 3f) :
            base(needsExitTime, enemy, exitTime)
        {
            this.BaseEnemyController = enemy;
            this.exitTime = exitTime;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = true;
            Agent.velocity = Vector3.zero;
            Animator.Play("Death");
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if (timer.Elapsed >= exitTime)
            {
                OnExit();
                BaseEnemyController.Spawner.DespawnAndRespawn(BaseEnemyController.gameObject);
            }
        }
        
    }
}