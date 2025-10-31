
using UnityEngine;
using UnityHFSM;
using UnityEngine.AI;

using Enemy.EnemyBehaviour;
using Enemy.EnemyBehaviour.States;
using Enemy.Sensor;


namespace  Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseEnemyController : MonoBehaviour
    {

        protected GameObject Player;
        protected Health PlayerHealthController;
        [SerializeField] protected string playerAttackColliderTag = "AttackCollider";
        [SerializeField] protected string playerTag = "Player";

        [Header("Enemy Model Sprite Renderer")] [SerializeField]
        protected SpriteRenderer spriteRenderer;


        [Header("Sensors")] [SerializeField] protected PlayerSensor followPlayerSensor;
        [SerializeField] protected PlayerSensor meleePlayerSensor;


        [Header("Attack Config")] [SerializeField] [Range(0.1f, 5f)]
        protected float attackCooldown = 2;

        [SerializeField] protected int attackDamage = 1;
        [SerializeField] protected float attackAnimExitTime;

        [Header("Hit Config")] [SerializeField]
        protected float hitAnimExitTime = 0.33f;
        
        [Header("Spawn Config")]
        [SerializeField] protected float spawnAnimExitTime = 3f;
        
        [Header("Death Config")]
        [SerializeField] protected float deathAnimExitTime = 3f;

        [Header("Patrol Config")] [SerializeField]
        protected float patrolSpeed;

        [SerializeField] protected float patrolMaxDistance;
        [SerializeField] protected float maxIdleTime;


        [Space] 
        [Header("Debug Info")] 
        [SerializeField] protected bool isInMeeleRange;
        public bool IsGotHit { get; set; }
        public bool IsDead { get; set; }
        public bool IsSpawned { get; set; } = false;


        [SerializeField] protected bool isInChasingRange;
        [SerializeField] protected float lastAttackTime;


        protected StateMachine<EnemyState, StateEvent> EnemyFsm;
        protected Animator Animator;
        protected NavMeshAgent Agent;

        public delegate void EnemyAttack();

        public static event EnemyAttack OnEnemyAttack;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponentInChildren<Animator>();
            EnemyFsm = new StateMachine<EnemyState, StateEvent>();
            Player = GameObject.FindWithTag(playerTag).gameObject;

            // Configure the agent for 2D
            Agent.updateUpAxis = false;
            Agent.updateRotation = false;

            //Add States
            EnemyFsm.AddState(EnemyState.Idle, new IdleState(false, this, maxIdleTime));
            EnemyFsm.AddState(EnemyState.Chase, new ChaseState(true, this, Player.transform));
            EnemyFsm.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack, attackAnimExitTime));
            EnemyFsm.AddState(EnemyState.Patrol, new PatrolState(true, this, patrolSpeed, patrolMaxDistance));
            EnemyFsm.AddState(EnemyState.Hit, new HitState(true, this, hitAnimExitTime));
            EnemyFsm.AddState(EnemyState.Death, new DeathState(true, this, deathAnimExitTime));
            EnemyFsm.AddState(EnemyState.Spawn, new SpawnState(true, this, spawnAnimExitTime));
            

            EnemyFsm.SetStartState(EnemyState.Spawn);

            //Add Transitions
            EnemyFsm.AddTriggerTransition(StateEvent.DetectPlayer,
                new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
            EnemyFsm.AddTriggerTransition(StateEvent.LostPlayer,
                new Transition<EnemyState>(EnemyState.Chase, EnemyState.Patrol));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
                (_) => isInChasingRange
                       && Vector3.Distance(Player.transform.position, transform.position) > Agent.stoppingDistance)
            );
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
                (_) => !isInChasingRange
                       || Vector3.Distance(Player.transform.position, transform.position) <= Agent.stoppingDistance)
            );
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Patrol));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Idle,
                (_) => Agent.remainingDistance <= 0));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Chase,
                (_) => isInChasingRange
                       && Vector3.Distance(Player.transform.position, transform.position) > Agent.stoppingDistance));


            //Attack Transitions
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee,
                forceInstantly: true));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee,
                forceInstantly: true));
            EnemyFsm.AddTransition(
                new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

            //Hit transitions
            //All states => Hit
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Hit, (_) => IsGotHit,
                forceInstantly: true));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Hit, (_) => IsGotHit,
                forceInstantly: true));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Hit, (_) => IsGotHit,
                forceInstantly: true));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Hit, (_) => IsGotHit,
                forceInstantly: true));
            
            //Hit => All states
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Hit, EnemyState.Chase, (_) => IsGotHit));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Hit, EnemyState.Idle, (_) => IsGotHit));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Hit, EnemyState.Patrol, (_) => IsGotHit));
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Hit, EnemyState.Attack, (_) => IsGotHit));
            
            //Death transitions
            //All stages => Death
            //From parameter can be anything
            EnemyFsm.AddTransitionFromAny(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Death, (_) => IsDead,
                forceInstantly: true));
            
            //Spawn transitions
            EnemyFsm.AddTransition(new Transition<EnemyState>(EnemyState.Spawn, EnemyState.Idle, (_) => IsSpawned));
            
            EnemyFsm.Init();
        }


        protected virtual void Start()
        {
            followPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
            followPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
            meleePlayerSensor.OnPlayerEnter += MeleePlayerSensor_OnPlayerEnter;
            meleePlayerSensor.OnPlayerExit += MeleePlayerSensor_OnPlayerExit;

            PlayerHealthController = Player.GetComponent<PlayerController>().Health;

        }

        protected void FollowPlayerSensor_OnPlayerEnter(Transform player)
        {

            EnemyFsm.Trigger(StateEvent.DetectPlayer);
            isInChasingRange = true;
        }

        protected void FollowPlayerSensor_OnPlayerExit(Vector2 lastKnownPosition)
        {
            EnemyFsm.Trigger(StateEvent.LostPlayer);
            isInChasingRange = false;
        }

        protected void MeleePlayerSensor_OnPlayerEnter(Transform player) => isInMeeleRange = true;
        protected void MeleePlayerSensor_OnPlayerExit(Vector2 lastKnownPosition) => isInMeeleRange = false;

        protected bool IsWithinIdleRange(Transition<EnemyState> transition) =>
            Agent.remainingDistance <= Agent.stoppingDistance;

        protected bool IsNotWithinIdleRange(Transition<EnemyState> transition) =>
            !IsWithinIdleRange(transition);

        protected bool ShouldMelee(Transition<EnemyState> transition) =>
            lastAttackTime + attackCooldown <= Time.time && isInMeeleRange;
        
        protected void OnAttack(State<EnemyState, StateEvent> state)
    {
        lastAttackTime = Time.time;
        PlayerHealthController.TakeDamage(attackDamage);
        OnEnemyAttack?.Invoke();
    }
    
    protected virtual void Update()
    {
        EnemyFsm.OnLogic();
        
        transform.rotation = Quaternion.identity;
        
        AdjustLookDirection(Agent.destination);
    }

    private void AdjustLookDirection(Vector3 destination)
    {
        if (!IsDead)
        {
            spriteRenderer.flipX = !(destination.x > transform.position.x);
        }
        
    }
}
}