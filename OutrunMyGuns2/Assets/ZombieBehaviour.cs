using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ZombieStates
{
    NoSpawned,
    Spawning,
    Walk,
    ThroughWall,
    Attack
}
public class ZombieBehaviour : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;
    public ZombieStates MyState;
    public Transform Target;

    [Header("Health")]
    public int Life = 100;

    [Header("Attack")]
    [SerializeField] Vector3 sphereTrigger;
    Vector3 spherePos
    {
        get { return transform.position + sphereTrigger; }
    }
    [SerializeField] float radius = 0.2f;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Attack();
        ManageState();
    }

    void ManageState()
    {
        switch (MyState)
        {
            case ZombieStates.NoSpawned:
                break;
            case ZombieStates.Spawning:
                break;
            case ZombieStates.Walk:
                nav.SetDestination(Target.position);
                break;
            case ZombieStates.ThroughWall:
                break;
            case ZombieStates.Attack:

                break;
            default:
                break;
        }
    }

    private void Attack()
    {
        RaycastHit hit;
        Vector3 pos = sphereTrigger + transform.position;
        if (Physics.SphereCast(spherePos, radius, transform.forward, out hit, radius) && MyState != ZombieStates.Attack)
        {
            if (hit.collider.TryGetComponent(out PlayerLife _pLife))
            {
                MyState = ZombieStates.Attack;
                nav.isStopped = true;
                anim.SetTrigger("Attack");
                _pLife.TakeDamage(50);

                Invoke(nameof(BackToNormalState), 2f);
            }
        }
    }

    void BackToNormalState()
    {
        nav.isStopped = false;
        MyState = ZombieStates.Walk;
    }
    public void TakeDamage(int _dmg)
    {
        Life -= _dmg;

        if (Life <= 0)
        {
            Debug.Log("MORT ");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(spherePos, radius);
    }
}
