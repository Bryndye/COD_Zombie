using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    StressReceiver camShake;
    PlayerController playerCtrl;

    [Header("Pos weapons")]
    public bool IsAiming = false;
    [SerializeField] Transform defaultPos, aimPos;
    [SerializeField] Transform weaponListT;
    [SerializeField] CameraRecoil camRecoil;


    [Header("Mes Armes")]
    [SerializeField] Transform camTransform;
    [SerializeField] Animator animHands;
    public List<Weapon> MyWeapons;
    public Weapon currentWeapon;

    [Header("Scope")]
    public float initFov;
    [SerializeField] Camera camWeapon;
    Vector3 difference;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI WeaponNameT;
    [SerializeField] TextMeshProUGUI MunChargT;
    [SerializeField] TextMeshProUGUI MunStockT;

    private void Awake()
    {
        camShake = GetComponentInChildren<StressReceiver>();
        playerCtrl = GetComponent<PlayerController>();
    }

    void Start()
    {
        for (int i = 0; i < weaponListT.childCount; i++)
        {
            MyWeapons.Add(weaponListT.GetChild(i).GetComponent<Weapon>());
        }
        currentWeapon = MyWeapons.FirstOrDefault();

        //initFov = camWeapon.fieldOfView;
    }

    void Update()
    {
        InputManager();
        UI();
    }

    void InputManager()
    {
        if (currentWeapon.canHold)
        {
            if (Input.GetAxisRaw("Fire1") != 0)
            {
                Fire();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Fire();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Scroll();
        }
        Aim();
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cut();
        }
    }

    private void UI()
    {
        WeaponNameT.text = currentWeapon.Name;
        MunChargT.text = currentWeapon.MunitionChargeur.ToString();
        MunStockT.text = currentWeapon.MunitionsStock.ToString();
    }

    #region Recoil
    [Header("Recoil")]
    public Transform RecoilHolder, RecoilHolderCam;

    Vector3 CurrentRecoil1, CurrentRecoil2, CurrentRecoil3, CurrentRecoil4;

    public float upRecoil, sideRocoil;

    private void Recoil()
    {

    }
    #endregion


    #region Input Handler
    public void Scroll()
    {
        int _index = MyWeapons.IndexOf(currentWeapon) + 1;
        if (_index >= MyWeapons.Count)
        {
            _index = 0;
        }
        foreach (var item in MyWeapons)
        {
            item.gameObject.SetActive(false);
        }
        currentWeapon = MyWeapons[_index];
        currentWeapon.gameObject.SetActive(true);

        difference = new Vector3(0, aimPos.position.y - currentWeapon.AimPos.position.y, 0);

    }

    public void Fire()
    {
        if (!currentWeapon.CanShoot)
            return;

        //camShake.InduceStress(0.05f); //pas sur de garder le shake !
        //camRecoil.RecoilFire();

        currentWeapon.MunitionChargeur--;
        for (int i = 0; i < currentWeapon.BulletsPerShoot; i++)
        {
            float x = Random.Range(-currentWeapon.Precision, currentWeapon.Precision);
            float y = Random.Range(-currentWeapon.Precision, currentWeapon.Precision);

            Vector3 _direction = camTransform.forward + new Vector3(x, y, 0);

            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, _direction, out hit))
            {
                //Debug.Log(hit.collider);
                if (hit.collider.TryGetComponent(out ZombieBehaviour _zb))
                {
                    _zb.TakeDamage(currentWeapon.Damage);
                }
            }
        }
        currentWeapon.CanShoot = false;
        currentWeapon.TimeToShoot = 0;
    }

    public void Reload()
    {
        if (currentWeapon.MunitionsStock > 0 && !currentWeapon.IsReloading)
        {
            currentWeapon.Reload();
        }
    }

    public void Aim()
    {
        if (playerCtrl.IsRunning || currentWeapon.IsReloading)
        {
            Debug.Log("NTM");
            return;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            IsAiming = true;
            Debug.Log(aimPos.position.y - currentWeapon.AimPos.position.y);
            currentWeapon.transform.position = aimPos.position - difference;
            //currentWeapon.transform.localPosition = Vector3.Slerp(currentWeapon.transform.localPosition, aimPos.localPosition - currentWeapon.AimPos.localPosition, initFov);
        }
        else
        {
            IsAiming = false;
            currentWeapon.transform.position = defaultPos.position;
            //currentWeapon.transform.localPosition = Vector3.Slerp(currentWeapon.transform.localPosition, defaultPos.localPosition, initFov);
        }
    }

    public void Cut()
    {

    }
    #endregion
}
