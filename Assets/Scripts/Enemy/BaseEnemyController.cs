using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
using UnityEngine.AI;
using System;
using Unity.AI.Navigation;

namespace  Enemy
{
[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class BaseEnemyController : MonoBehaviour
{
  
    private GameObject Player;
    private Health PlayerHealthController;


    [Header("Sensors")]
    [SerializeField]
    private PlayerSensor FollowPlayerSensor;
    [SerializeField]
    private PlayerSensor MeleePlayerSensor;

    [Header("Attack Config")]
    [SerializeField]
    [Range(0.1f, 5f)]
    private float AttackCooldown = 2;
    [SerializeField]
    private int AttackDamage = 1;

    [Header("Patrol Config")]
    [SerializeField]
    private float PatrolSpeed;
    [SerializeField]
    private float PatrolMaxDistance;
    [SerializeField]
    private float maxIdleTime;

    [Space]
    [Header("Debug Info")]
    [SerializeField]
    private bool IsInMeeleRange;
    [SerializeField]
    private bool IsInChasingRange;
    [SerializeField]
    private float LastAttackTime;
    

    private StateMachine<EnemyState, StateEvent> EnemyFSM;
    private Animator Animator;
    private NavMeshAgent Agent;

    public delegate void EnemyAttack();
    public static event EnemyAttack OnEnemyAttack;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        EnemyFSM = new StateMachine<EnemyState, StateEvent>();
        Player = GameObject.FindWithTag("Player").GetComponentInChildren<CharacterController>().gameObject;
        PlayerHealthController = Player.GetComponent<Health>();

        //Add States
        EnemyFSM.AddState(EnemyState.Idle, new IdleState(false, this, maxIdleTime));
        EnemyFSM.AddState(EnemyState.Chase, new ChaseState(true, this, Player.transform));
        EnemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack));
        EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(true, this, PatrolSpeed, PatrolMaxDistance));

        EnemyFSM.SetStartState(EnemyState.Idle);

        //Add Transitions
        EnemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase));
        EnemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase,
            (transition) => IsInChasingRange
            && Vector3.Distance(Player.transform.position, transform.position) > Agent.stoppingDistance)
            );
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Idle,
            (transition) => !IsInChasingRange
            || Vector3.Distance(Player.transform.position, transform.position) <= Agent.stoppingDistance)
            );
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Patrol));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Idle,
            (transition) => Agent.remainingDistance <= 0));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Chase,
            (transition) => IsInChasingRange
            && Vector3.Distance(Player.transform.position, transform.position) > Agent.stoppingDistance));


        //Attack Transitions
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee, forceInstantly: true));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee, forceInstantly: true));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, IsNotWithinIdleRange));
        EnemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));

        
        EnemyFSM.Init();
    }

   
    private void Start()
    {
        FollowPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerEnter;
        FollowPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerExit;
        MeleePlayerSensor.OnPlayerEnter += MeleePlayerSensor_OnPlayerEnter;
        MeleePlayerSensor.OnPlayerExit += MeleePlayerSensor_OnPlayerExit;
        
        
    }

    private void FollowPlayerSensor_OnPlayerEnter(Transform Player) 
    {
        
        EnemyFSM.Trigger(StateEvent.DetectPlayer);
        IsInChasingRange = true; 
    }
    private void FollowPlayerSensor_OnPlayerExit(Vector3 LastKnownPosition)
    {
        EnemyFSM.Trigger(StateEvent.LostPlayer);
        IsInChasingRange = false;
    }

    private void MeleePlayerSensor_OnPlayerEnter(Transform Player) => IsInMeeleRange = true;
    private void MeleePlayerSensor_OnPlayerExit(Vector3 LastKnownPosition) => IsInMeeleRange = false;

    private bool IsWithinIdleRange(Transition<EnemyState> Transition) =>
        Agent.remainingDistance <= Agent.stoppingDistance;

    private bool IsNotWithinIdleRange(Transition<EnemyState> Transition) =>
       !IsWithinIdleRange(Transition);

    private bool ShouldMelee(Transition<EnemyState> Transition) =>
        LastAttackTime + AttackCooldown <= Time.time && IsInMeeleRange;

    private void OnAttack(State<EnemyState, StateEvent> State)
    {
        transform.LookAt(Player.transform.position);
        LastAttackTime = Time.time;
        PlayerHealthController.TakeDamage(AttackDamage);
        OnEnemyAttack?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {   
            
            
        }
    }

    private void Update()
    {
        EnemyFSM.OnLogic();
       // Debug.Log(EnemyFSM.ActiveState);
    }
}
}