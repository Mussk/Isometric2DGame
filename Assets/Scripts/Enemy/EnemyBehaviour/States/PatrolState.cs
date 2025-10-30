
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class PatrolState : EnemyStateBase
{

    private float CachedAgentSpeed;
    private float PatrolSpeed;
    private float PatrolMaxDistance;

    private Vector3 PatrolDestination;

    public PatrolState(bool needsExitTime, BaseEnemyController Enemy,
       float PatrolSpeed, float PatrolMaxDistance)
        : base(needsExitTime, Enemy)
    {
        this.PatrolSpeed = PatrolSpeed;
        this.PatrolMaxDistance = PatrolMaxDistance;
        PatrolDestination = Vector3.zero;
    }

    public override void OnEnter()
    {

        CachedAgentSpeed = Agent.speed;
        Agent.speed = PatrolSpeed;
        Agent.enabled = true;
        Agent.isStopped = false;
        PatrolDestination = GetRandomNavMeshPosition(Enemy.gameObject.transform.position, PatrolMaxDistance);
        base.OnEnter();
        Animator.Play("Walk");

    }

    public override void OnLogic() 
    {   
       
        
        if (!RequestedExit) 
        {
            
            
            Agent.SetDestination(PatrolDestination);
        
        }
        else if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            
            fsm.StateCanExit();

        }
        base.OnLogic();

    }

    public override void OnExit()
    {
        base.OnExit();
        Agent.speed = CachedAgentSpeed;
    }

    Vector3 GetRandomNavMeshPosition(Vector3 center, float maxDistance)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * maxDistance;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero; // Return zero vector if no valid position found
    }
}
