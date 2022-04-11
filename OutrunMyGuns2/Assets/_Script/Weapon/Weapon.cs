using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name = "M1911";

    public Animator Anim;

    [SerializeReference] public ScriptableWeaponStat stat;

    [Header("Munitions")]
    public int AmmoMaxStock = 80;
    public int AmmoMaxMag = 8;
    public int AmmoStock = 36, AmmoMag = 8;
    [HideInInspector] public bool IsReloading = false;
    public float ReloadTime = 1f;

    [Header("Fire")]
    public int Damage = 10;
    public int BulletsPerShoot = 1;
    public float FireRate = 0.4f;
    [HideInInspector] public bool CanShoot = false;
    [HideInInspector] public float TimeToShoot = 0;
    [Tooltip("Plus proche de 1 plus precis, plus proche de 0 moins precis")]
    public float Precision = 0.1f;
    public bool canHold = true;
     public AudioClip ClipShoot;

    [Header("Recoil")]
    public Vector3 recoil;
    public float snapiness, returnSpeed;

    [Header("Aim")]
    public Transform AimPos;
    public int AimFov;
    public float SpeedToScoop;

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
        else if (!CanShoot && AmmoMag > 0)
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
            IsReloading = true;
            Invoke(nameof(GetAmmo), ReloadTime / _mutli);
        }
    }

    private void GetAmmo()
    {
        //Debug.Log("reload done !" + gameObject);
        int _needAmmo = AmmoMaxMag - AmmoMag;
        if (_needAmmo >= AmmoStock)
        {
            _needAmmo = AmmoStock;
            AmmoStock -= _needAmmo;
        }
        else
        {
            AmmoStock -= _needAmmo;
        }
        AmmoMag += _needAmmo;

        IsReloading = false;
    }

    //Mise en place de rechargement darmes balle par balle (shotgun)
    //on peut cut et recevoir les munitions ! Il ny a pas de cancel anim CancelInvoke();
    public void GetAmmoOneByOne()
    {
        Anim.SetTrigger("Reload");
        IsReloading = true;
        AmmoStock--;
        AmmoMag++;

        IsReloading = false;
    }


    public void MaxAmmo()
    {
        AmmoStock = AmmoMaxStock;
        AmmoMag = AmmoMaxMag;
    }

}
