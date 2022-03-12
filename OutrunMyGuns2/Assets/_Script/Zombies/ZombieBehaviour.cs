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

    [SerializeField] Vector3 posPassThroughWindow;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        //myColliders = GetComponent<Collider>();
        waveMan = WaveManager.Instance;
    }

    private void Start()
    {
       //ChooseMySpeed();
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
        if (MyState == ZombieStates.Dead)
        {
            return;
        }
        if (MyState == ZombieStates.Walk || MyState == ZombieStates.Run)
        {
            if (Target != null)
            {
                nav.SetDestination(Target.position);
                RaycastFindPlayerToAttack();
            }
            else
            {

            }
        }
        else if (MyState == ZombieStates.ThroughWall)
        {
            PassThroughTheWindow();
        }
    }

    private bool IsAWindow()
    {
        RaycastHit hit;
        if (Physics.Raycast(directionAttack, transform.forward, out hit, distanceDetection))
        {
            if (hit.collider.TryGetComponent(out Window _w))
            {
                return true;
            }
        }
        return false;
    }

    private void PassThroughTheWindow()
    {
        transform.position = Vector3.Lerp(transform.position, posPassThroughWindow, 0.02f);
        if (Vector3.Distance(transform.position, posPassThroughWindow) < 0.1f)
        {
            BackToNormalState();
        }
    }


    #region Attack
    private void RaycastFindPlayerToAttack()
    {
        //Debug.Log("atack");
        RaycastHit hit;

        if (Physics.Raycast(directionAttack, transform.forward, out hit, distanceDetection) && MyState != ZombieStates.Attack && MyState != ZombieStates.ThroughWall)
        {
            //Debug.Log("Je collide tout");
            if (hit.collider.TryGetComponent(out PlayerLife _pLife))
            {
                //Debug.Log("Je collide player");
                MyState = ZombieStates.Attack;
                nav.isStopped = true;
                anim.SetTrigger("Attack");
            }
            else if (hit.collider.TryGetComponent(out Window _w))
            {
                if (_w.plankStill <= 0)
                {
                    anim.SetTrigger("Climb");
                    nav.isStopped = true;
                    nav.enabled = false;
                    posPassThroughWindow = _w.finalPosZombie.position;
                    MyState = ZombieStates.ThroughWall;
                }
                else
                {
                    MyState = ZombieStates.Attack;
                    nav.isStopped = true;
                    anim.SetTrigger("AttackWindow");
                }
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
            else if (hit.collider.TryGetComponent(out Window _w))
            {
                _w.TakeDamage();
            }
        }
    }
    #endregion

    public void BackToNormalState()
    {
        nav.enabled = true;
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
        if (nav.enabled)
        {
            nav.isStopped = true;
        }
        nav.enabled = false;

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
