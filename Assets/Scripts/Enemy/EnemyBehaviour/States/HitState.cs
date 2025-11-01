using System;
using UnityHFSM;

namespace Enemy.EnemyBehaviour.States
{
    public class HitState : EnemyStateBase
    {
        private BaseEnemyController baseEnemyController;

        public HitState(bool needsExitTime,
            BaseEnemyController enemy,
            float exitTime = 0.33f)
            : base(needsExitTime, enemy, exitTime)
        {

            this.baseEnemyController = enemy;

        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = true;
            Animator.Play("Hit");
        }

        public override void OnExit()
        {
            base.OnExit();
            baseEnemyController.IsGotHit = false;
        }
    }
}