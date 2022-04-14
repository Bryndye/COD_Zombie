using UnityEngine;
using UnityEngine.AI;

public enum ZombieStates
{
    Idle,
    Walk,
    Run,
    Crawl,
    ThroughWall,
    Attack,
}
public class ZombieBehaviour : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;
    ZombieAudioManager zbAudio;
    BonusManager bonusManager;
    WaveManager waveManager;

    [SerializeField] Collider[] myColliders;
    public ZombieStates MyState;
    [HideInInspector] public ZombieStates myNormalStates;

    [Header("Health")]
    public int Life = 100;
    public bool IsDead = false;

    [Header("Movement")]
    public Transform Target, WindowTarget;
    [SerializeField] bool pathComplete;
    Vector3 posPassThroughWindow;
    
    [Header("Attack")]
    [SerializeField] LayerMask ignoreLayerAttack;
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
        zbAudio = GetComponent<ZombieAudioManager>();
    }

    private void Start()
    {
        waveManager = WaveManager.Instance;
        bonusManager = BonusManager.Instance;
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
        anim.SetBool("Walk1", _walkAnim);
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

    void CrawlSpeed()
    {
        anim.SetBool("Crawl", true);
        MyState = ZombieStates.Walk;
        nav.speed = 1.5f;
    }

    void IdleSpeed()
    {
        anim.SetBool("Idle", true);
        MyState = ZombieStates.Idle;
        nav.speed = 0;
    }
    #endregion

    void Update()
    {
        if (IsDead)
        {
            return;
        }

        ManageState();
        IfPlayerBesideMe();
    }

    void ManageState()
    {
        if (MyState == ZombieStates.Walk || MyState == ZombieStates.Run)
        {
            NavigateToTarget();
            RaycastFindPlayerToAttack();
        }
        else if (MyState == ZombieStates.ThroughWall)
        {
            PassThroughTheWindow();
        }
        else if(MyState == ZombieStates.Idle)
        {
            anim.SetBool("Idle", true);
        }
    }

    private void NavigateToTarget()
    {
        if (!nav.enabled)
        {
            return;
        }
        Vector3 _targetPos = transform.position;

        if (WindowTarget != null)
        {
            _targetPos = WindowTarget.position;
        }
        else if (Target != null)
        {
            _targetPos = Target.position;
        }

        if (NearMyTarget(_targetPos))
        {
            return;
        }
        nav.SetDestination(GetPath(_targetPos));
    }

    private Vector3 GetPath(Vector3 _pos)
    {
        if (!pathComplete && nav.pathStatus == NavMeshPathStatus.PathComplete || nav.hasPath && nav.pathStatus == NavMeshPathStatus.PathPartial)
        {
            pathComplete = true;
            return nav.destination;
        }
        else
        {
            pathComplete = false;
            return _pos;
        }
    }

    private bool NearMyTarget(Vector3 _target)
    {
        return Vector3.Distance(transform.position, _target) < 1f;
    }

    #region Window Manager
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
            WindowTarget = null;
            BackToNormalState();
        }
    }
    #endregion

    private void IfPlayerBesideMe()
    {
        if (Vector3.Distance(transform.position, Target.position) < 1)
        {
            Vector3 _dir = new Vector3(Target.position.x, transform.position.y, Target.position.z);
            transform.LookAt(_dir);
            nav.enabled = false;
        }
    }


    #region Attack
    private void RaycastFindPlayerToAttack()
    {
        RaycastHit hit;

        if (Physics.Raycast(directionAttack, transform.forward, out hit, distanceDetection, ~ignoreLayerAttack) && MyState != ZombieStates.Attack && MyState != ZombieStates.ThroughWall)
        {
            if (hit.collider.TryGetComponent(out PlayerLife _pLife))
            {
                MyState = ZombieStates.Attack;
                nav.isStopped = true;
                anim.SetTrigger("Attack");
                zbAudio.SoundAttack();
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
        if (Physics.Raycast(directionAttack, transform.forward, out hit, distanceDetection, ~ignoreLayerAttack))
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


    #region Health system
    public void TakeDamage(int _dmg, PlayerPoints _player, TypeKill _type = TypeKill.normal)
    {
        if (IsDead)
        {
            return;
        }


        if (bonusManager != null && bonusManager.IsInstaKill)
        {
            Life = 0;
        }
        else
        {
            Life -= _dmg;
        }

        if (Life <= 0)
        {
            Dying();
            _player.GetPointsByHit(_type);
        }
        else
        {
            _player.GetPointsByHit(TypeKill.None);
        }
    }

    public void Dying()
    {
        //Debug.Log("MORT ");
        IsDead = true;
        anim.SetTrigger("Dying");
        zbAudio.SoundDeath();
        if (nav.enabled)
        {
            nav.isStopped = true;
        }
        nav.enabled = false;

        waveManager.RemoveZombie(this);
        Invoke(nameof(DisableZombie), 10);

        foreach (var item in myColliders)
        {
            item.enabled = false;
        }

        if (bonusManager != null && WindowTarget == null)
        {
            bonusManager.SpawnBonus(transform.position);
        }
    }

    public void Reviving()
    {
        anim.SetTrigger("Revive");
        anim.ResetTrigger("Revive");
        IsDead = false;

        BackToNormalState();
        foreach (var item in myColliders)
        {
            item.enabled = true;
        }
    }
    #endregion


    #region Other voids
    public void BackToNormalState()
    {
        nav.enabled = true;
        nav.isStopped = false;
        MyState = myNormalStates;
    }
    private void DisableZombie()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(directionAttack, transform.forward,Color.red);
    }
    #endregion
}
