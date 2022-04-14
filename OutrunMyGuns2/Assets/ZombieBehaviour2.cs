using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehaviour2 : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;
    ZombieAudioManager zbAudio;
    BonusManager bonusManager;
    WaveManager waveManager;

    public ZombieState[] AllZombieStates;
    [SerializeField] Collider[] myColliders;
    public ZombieStates MyState;

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
    [SerializeField] float distanceDetection = 1.5f;


    void Update()
    {
        
    }

    private void ChangeState(ZombieStates _state)
    {

    }

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
        IsDead = true;
        anim.SetTrigger("Dying");
        zbAudio.SoundDeath();
        if (nav.enabled)
        {
            nav.isStopped = true;
        }
        nav.enabled = false;

        //waveManager.RemoveZombie(this);
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
    }
    private void DisableZombie()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(directionAttack, transform.forward, Color.red);
    }
    #endregion
}

[System.Serializable]
public class ZombieState
{
    public ZombieStates State;
    public float Speed;
    public string StringAnim = "Walk";
}
