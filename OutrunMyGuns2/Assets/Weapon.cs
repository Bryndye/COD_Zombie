using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name = "M1911";

    public Animator Anim;

    [Header("Munitions")]
    public int MunitionsMaxStock = 80;
    public int MunitionsMaxChargeur = 8;
    public int MunitionsStock = 36, MunitionChargeur = 8;
    [HideInInspector] public bool IsReloading = false;

    [Header("Fire")]
    [HideInInspector] public bool CanShoot = false;
    public int Damage = 10;
    public int BulletsPerShoot = 1;
    public float FireRate = 0.4f;
    [HideInInspector] public float TimeToShoot = 0;
    [Tooltip("Plus proche de 1 plus precis, plus proche de 0 moins precis")]
    public float Precision = 0.1f;
    public bool canHold = true;
    [HideInInspector] public AudioSource SFX;

    [Header("Recoil")]
    public Vector3 recoil;
    public float snapiness, returnSpeed;

    [Header("Aim")]
    public Transform AimPos;
    public int AimFov;
    public float SpeedToScoop;

    private void Awake()
    {
        SFX = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        CanShoot = false;
        TimeToShoot = 0;
    }

    private void Update()
    {
        if (IsReloading)
        {
            CanShoot = false;
        }
        else if (!CanShoot && MunitionChargeur > 0)
        {
            TimeToShoot += Time.deltaTime;
            if (TimeToShoot >= FireRate)
            {
                CanShoot = true;
            }
        }
    }

    public void Reload(float _mutli = 1)
    {
        if (Anim != null)
        {
            IsReloading = true;
            Anim.SetFloat("Multi", _mutli);
            Anim.SetTrigger("Reload");
        }
        else
        {
            GetAmmo();
        }
    }

    private void GetAmmo()
    {
        int _needAmmo = MunitionsMaxChargeur - MunitionChargeur;
        if (_needAmmo >= MunitionsStock)
        {
            _needAmmo = MunitionsStock;
            MunitionsStock -= _needAmmo;
        }
        else
        {
            MunitionsStock -= _needAmmo;
        }
        MunitionChargeur += _needAmmo;

        IsReloading = false;
    }


    public void MaxAmmo()
    {
        MunitionsStock = MunitionsMaxStock;
        MunitionChargeur = MunitionsMaxChargeur;
    }

}
