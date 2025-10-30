using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class ChaseState : EnemyStateBase
{
    private Transform Target;

    public ChaseState(bool needsExitTime, BaseEnemyController Enemy, Transform Target) : base(needsExitTime, Enemy)
    {
        this.Target = Target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Agent.enabled = true;
        Agent.isStopped = false;

        Animator.Play("Run");
    }

    public override void OnLogic() 
    {

        base.OnLogic();
        
        if (!RequestedExit)
        {
            
            Agent.SetDestination(Target.position);

            
        }
        else if (Agent.remainingDistance <= Agent.stoppingDistance) 
        {

            fsm.StateCanExit();

        }
    }
}
