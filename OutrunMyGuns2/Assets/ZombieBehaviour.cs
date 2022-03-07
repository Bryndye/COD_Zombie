using UnityEngine;
using UnityEngine.AI;

public enum ZombieStates
{
    Walk,
    ThroughWall,
    Attack,
    Dead
}
public class ZombieBehaviour : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;
    CapsuleCollider cc;
    public ZombieStates MyState;
    public Transform Target;

    [Header("Health")]
    public int Life = 100;

    [Header("Attack")]
    [SerializeField] Vector3 sphereTrigger;
    Vector3 directionAttack
    {
        get { return transform.position + transform.TransformDirection(sphereTrigger); }
    }
    [SerializeField] float distanceDetection = 1f;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        bool _isWalk = Random.Range(0, 2) == 1 ? true : false;
        anim.SetBool("WalkWay1", _isWalk);
    }

    void Update()
    {
        ManageState();
    }

    void ManageState()
    {
        switch (MyState)
        {
            case ZombieStates.Dead:
                break;
            case ZombieStates.Walk:
                nav.SetDestination(Target.position);
                Attack();
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
        Debug.Log("atack");
        RaycastHit hit;
        Vector3 pos = sphereTrigger + transform.position;
        if (Physics.Raycast(directionAttack, transform.forward, out hit, distanceDetection) && MyState != ZombieStates.Attack)
        {
            Debug.Log("Je collide tout");
            if (hit.collider.TryGetComponent(out PlayerLife _pLife))
            {
                Debug.Log("Je collide player");
                MyState = ZombieStates.Attack;
                nav.isStopped = true;
                anim.SetTrigger("Attack");
            }
        }
    }

    public void Scratch()
    {
        RaycastHit hit;
        if (Physics.Raycast(directionAttack, transform.forward, out hit, distanceDetection))
        {
            if (hit.collider.TryGetComponent(out PlayerLife _pLife))
            {
                _pLife.TakeDamage(50);
            }
        }
    }

    public void BackToNormalState()
    {
        nav.isStopped = false;
        MyState = ZombieStates.Walk;
    }
    public void TakeDamage(int _dmg)
    {
        Life -= _dmg;

        if (Life <= 0)
        {
            DyingReviving(true);
        }
    }

    public void DyingReviving(bool _active)
    {
        Debug.Log("MORT ");
        MyState = ZombieStates.Dead;
        anim.SetTrigger("Dying");
        nav.isStopped = _active;
        cc.enabled = !_active;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(directionAttack, transform.forward,Color.red);
    }
}
