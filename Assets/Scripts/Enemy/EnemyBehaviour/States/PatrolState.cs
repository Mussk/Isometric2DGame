using UnityEngine;
using UnityEngine.AI;

namespace Enemy.EnemyBehaviour.States
{
    public class PatrolState : EnemyStateBase
    {

        private float _cachedAgentSpeed;
        private float _patrolSpeed;
        private float _patrolMaxDistance;

        private Vector3 _patrolDestination;

        public PatrolState(bool needsExitTime, BaseEnemyController enemy,
            float patrolSpeed, float patrolMaxDistance)
            : base(needsExitTime, enemy)
        {
            this._patrolSpeed = patrolSpeed;
            this._patrolMaxDistance = patrolMaxDistance;
            _patrolDestination = Vector3.zero;
        }

        public override void OnEnter()
        {

            _cachedAgentSpeed = Agent.speed;
            Agent.speed = _patrolSpeed;
            Agent.enabled = true;
            Agent.isStopped = false;
            _patrolDestination = GetRandomNavMeshPosition(Enemy.gameObject.transform.position, _patrolMaxDistance);
            base.OnEnter();
            Animator.Play("Movement");

        }

        public override void OnLogic() 
        {   
       
        
            if (!RequestedExit) 
            {
            
            
                Agent.SetDestination(_patrolDestination);
        
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
            Agent.speed = _cachedAgentSpeed;
        }

        private Vector3 GetRandomNavMeshPosition(Vector3 center, float maxDistance)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * maxDistance;
            return NavMesh.SamplePosition(randomPoint, out var hit, maxDistance, NavMesh.AllAreas) ? hit.position : Vector3.zero;
        }
    }
}
