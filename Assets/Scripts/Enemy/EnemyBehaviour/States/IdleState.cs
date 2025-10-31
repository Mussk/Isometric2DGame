namespace Enemy.EnemyBehaviour.States
{
    public class IdleState : EnemyStateBase
    {
        public IdleState(bool needsExitTime, BaseEnemyController enemy, float exitTime = 3.0f) : base(needsExitTime, enemy, exitTime) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Agent.isStopped = true;
            Animator.Play("Idle");
        }
    }
}
