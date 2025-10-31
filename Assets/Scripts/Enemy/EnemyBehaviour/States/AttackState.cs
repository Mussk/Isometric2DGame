using System;
using UnityEngine;
using UnityHFSM;

namespace Enemy.EnemyBehaviour.States
{
    public class AttackState : EnemyStateBase
    {
        public AttackState(bool needsExitTime,
            BaseEnemyController enemy,
            Action<State<EnemyState, StateEvent>> onEnter,
            float exitTime = 1f) : 
            base(needsExitTime, enemy, exitTime, onEnter) { }

        public override void OnEnter()
        {
            
            Agent.isStopped = true;
            base.OnEnter();
            Animator.Play("Attack");
            
            
        }

       
    }
}
