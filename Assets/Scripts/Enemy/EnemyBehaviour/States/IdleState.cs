using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, BaseEnemyController Enemy, float ExitTime = 3.0f) : base(needsExitTime, Enemy, ExitTime) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Agent.isStopped = true;
        //Animator.Play("Idle_A");
    }

    public override void OnLogic()
    {
       // AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(0);

        base.OnLogic();
    }
}
