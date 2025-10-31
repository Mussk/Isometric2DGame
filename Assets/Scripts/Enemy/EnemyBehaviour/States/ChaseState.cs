using UnityEngine;

namespace Enemy.EnemyBehaviour.States
{
    public class ChaseState : EnemyStateBase
    {
        private Transform _target;

        public ChaseState(bool needsExitTime, BaseEnemyController enemy, Transform target) : base(needsExitTime, enemy)
        {
            this._target = target;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.enabled = true;
            Agent.isStopped = false;

            Animator.Play("Movement");
        }

        public override void OnLogic() 
        {

            base.OnLogic();
        
            if (!RequestedExit)
            {
            
                Agent.SetDestination(_target.position);

            
            }
            else if (Agent.remainingDistance <= Agent.stoppingDistance) 
            {

                fsm.StateCanExit();

            }
        }
    }
}
