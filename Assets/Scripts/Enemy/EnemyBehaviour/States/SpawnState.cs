
namespace Enemy.EnemyBehaviour.States
{
    public class SpawnState : EnemyStateBase
    {
        protected BaseEnemyController baseEnemyController;
        protected float exitTime;

        public SpawnState(bool needsExitTime, BaseEnemyController enemy, float exitTime = 3.0f)
            : base(needsExitTime, enemy, exitTime)
        {
            baseEnemyController = enemy;
            this.exitTime = exitTime;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = true;
            Animator.Play("Spawn");
        }
        
        public override void OnLogic()
        {
            base.OnLogic();

            if (timer.Elapsed >= exitTime)
            {
                baseEnemyController.IsSpawned = true;
            }
            
        }

        public override void OnExit()
        {
            base.OnExit();
            Agent.isStopped = false;
        }
    }
}

