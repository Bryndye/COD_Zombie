using UnityEngine;
using UnityEngine.AI;

public enum ZombieStates
{
    Walk,
    Run,
    ThroughWall,
    Attack,
    Dead
}
public class ZombieBehaviour : MonoBehaviour
{
    WaveManager waveMan;
    NavMeshAgent nav;
    Animator anim;
    [SerializeField] Collider[] myColliders;
    public ZombieStates MyState;
    [HideInInspector] public ZombieStates myNormalStates;
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
        //myColliders = GetComponent<Collider>();
        waveMan = WaveManager.Instance;
    }

    private void Start()
    {
        ChooseMySpeed();
    }

    #region Stat Move
    public void ChooseMySpeed()
    {
        if (myNormalStates == ZombieStates.Walk)
        {
            WalkSpeed();
        }
        else if (myNormalStates == ZombieStates.Run)
        {
            RunSpeed();
        }
    }

    void WalkSpeed()
    {
        bool _walkAnim = Random.Range(0, 2) == 1 ? true : false;
        anim.SetBool("WalkWay1", _walkAnim);
        MyState = ZombieStates.Walk;
        nav.speed = 1;
    }

    void RunSpeed()
    {
        //bool _runAnim = Random.Range(0, 2) == 1 ? true : false;
        anim.SetBool("Run", true);
        MyState = ZombieStates.Run;
        nav.speed = 3;
    }
    #endregion

    void Update()
    {
        ManageState();
    }

    void ManageState()
    {
        if (MyState == ZombieStates.Walk || MyState == ZombieStates.Run)
        {
            nav.SetDestination(Target.position);
            RaycastFindPlayerToAttack();
        }
    }

    #region Attack
    private void RaycastFindPlayerToAttack()
    {
        //Debug.Log("atack");
        RaycastHit hit;

        if (Physics.Raycast(directionAttack, transform.forward, out hit, distanceDetection) && MyState != ZombieStates.Attack)
        {
            //Debug.Log("Je collide tout");
            if (hit.collider.TryGetComponent(out PlayerLife _pLife))
            {
                //Debug.Log("Je collide player");
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
                _pLife.TakeDamage(50, transform);
            }
        }
    }
    #endregion

    public void BackToNormalState()
    {
        nav.isStopped = false;
        MyState = myNormalStates;
    }
    public void TakeDamage(int _dmg, PlayerWeapon _player, bool _isHead = false)
    {
        if (MyState == ZombieStates.Dead)
        {
            return;
        }
        Life -= _dmg;

        if (Life <= 0)
        {
            Dying();
            if (_isHead)
                _player.FeedbackHitZombie(100);
            else
                _player.FeedbackHitZombie(50);
        }
        else
        {
            _player.FeedbackHitZombie(10);
        }
    }

    public void Dying()
    {
        //Debug.Log("MORT ");
        MyState = ZombieStates.Dead;
        anim.SetTrigger("Dying");
        nav.isStopped = true;
        waveMan.RemoveZombie(this);
        Invoke(nameof(DisableZombie), 10);

        foreach (var item in myColliders)
        {
            item.enabled = false;
        }
    }

    public void Reviving()
    {
        anim.SetTrigger("Revive");
        anim.ResetTrigger("Revive");

        BackToNormalState();
        foreach (var item in myColliders)
        {
            item.enabled = true;
        }
    }

    private void DisableZombie()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(directionAttack, transform.forward,Color.red);
    }
}
