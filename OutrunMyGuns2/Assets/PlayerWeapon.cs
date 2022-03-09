using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    PlayerController playerCtrl;
    PlayerPoints playerPoints;

    [Header("Pos weapons")]
    public bool IsAiming = false;
    [SerializeField] Transform defaultPos, aimPos;
    [SerializeField] Transform weaponListT;
    [SerializeField] CameraRecoil camRecoil;


    [Header("Mes Armes")]
    [SerializeField] Transform camTransform;
    [SerializeField] LayerMask ignoreLayerShoot;
    [SerializeField] Animator animHands;
    public List<Weapon> MyWeapons;
    public Weapon currentWeapon;
    public int CountMaxWeapons;
    public GameObject PREFAB_TEST_SHOOT;

    [Header("Scope")]
    public float FovDefault;
    [SerializeField] Camera camWeapon;
    Vector3 difference { get { return new Vector3(0, currentWeapon.AimPos.localPosition.y, 0); } }
    float coefAim = 1, coefMovement = 1;

    [Header("Perks Effets")]
    public float MultiplicateurBullets = 1;
    public float MultiplicateurSpeedReload = 1;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI WeaponNameT;
    [SerializeField] TextMeshProUGUI MunChargT;
    [SerializeField] TextMeshProUGUI MunStockT;
    [SerializeField] GameObject Hitmarker, Reticules;


    public List<float> X, Y;
    public float MoyX, MoyY, SommeX, SommeY;


    private void Awake()
    {
        playerCtrl = GetComponent<PlayerController>();
        playerPoints = GetComponent<PlayerPoints>();
    }

    void Start()
    {
        for (int i = 0; i < weaponListT.childCount; i++)
        {
            MyWeapons.Add(weaponListT.GetChild(i).GetComponent<Weapon>());
        }
        currentWeapon = MyWeapons.FirstOrDefault();
    }

    void Update()
    {
        InputManager();
        UI();
    }

    void InputManager()
    {
        ModificateurAim();
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cut();
        }
        Aim();
    }

    private void ModificateurAim()
    {
        coefAim = IsAiming ? 2 : 1;
        switch (playerCtrl.MovementState)
        {
            case MovementState.Idle:
                coefMovement = 1;
                break;
            case MovementState.Walk:
                coefMovement = 0.5f;
                break;
            case MovementState.Crouch:
                coefMovement = 1.5f;
                break;
            default:
                break;
        }
    }

    private void UI()
    {
        WeaponNameT.text = currentWeapon.Name;
        MunChargT.text = currentWeapon.MunitionChargeur.ToString();
        MunStockT.text = currentWeapon.MunitionsStock.ToString();

        Reticules.SetActive(!IsAiming);
    }


    private void HitmarkerActiveUI()
    {
        Hitmarker.SetActive(false);
        Hitmarker.SetActive(true);
    }

    public void FeedbackHitZombie(int _p)
    {
        HitmarkerActiveUI();
        playerPoints.GetPoints(_p);
    }


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
    }

    public void Fire()
    {
        if (!currentWeapon.CanShoot || playerCtrl.MovementState == MovementState.Run)
            return;

        camRecoil.RecoilFire();

        currentWeapon.MunitionChargeur--;
        if (currentWeapon.SFX != null)
            currentWeapon.SFX.Play();

        for (int i = 0; i < currentWeapon.BulletsPerShoot * MultiplicateurBullets; i++)
        {
            float _x = Random.Range(-currentWeapon.Precision, currentWeapon.Precision) / (coefAim + coefMovement);
            float _y = Random.Range(-currentWeapon.Precision, currentWeapon.Precision) / (coefAim + coefMovement);

            Vector3 _direction = camTransform.forward;
            _direction = Quaternion.Euler(0, camTransform.localRotation.y + _y, camTransform.localRotation.x + _x) * _direction;

            //_direction += Quaternion.AngleAxis(Random.Range(-currentWeapon.Precision, currentWeapon.Precision), Vector3.forward) * _direction;
            //_direction += Quaternion.AngleAxis(Random.Range(-currentWeapon.Precision, currentWeapon.Precision), Vector3.up) * _direction;

            Moyenne(_x,_y);

            RaycastHit[] hits = Physics.RaycastAll(camTransform.position, _direction, 100, ~ignoreLayerShoot);
            for (int y = 0; y < hits.Length; y++)
            {
                if (hits[y].collider.TryGetComponent(out ZombieBehaviour _zb))
                {
                    _zb.TakeDamage(currentWeapon.Damage, this);
                }
                else if (hits[y].collider.TryGetComponent(out PartOfBody _head))
                {
                    _head.TakeDamage(currentWeapon.Damage, this);
                }
                else
                {
                    //Debug.Log(hit.collider.name);
                    Instantiate(PREFAB_TEST_SHOOT, hits[y].point, Quaternion.identity);
                }
            }

            Debug.DrawRay(camTransform.position, _direction * 10, Color.green, 1);
        }
        currentWeapon.CanShoot = false;
        currentWeapon.TimeToShoot = 0;
    }

    public void Reload()
    {
        if (currentWeapon.MunitionsStock > 0 && !currentWeapon.IsReloading)
        {
            currentWeapon.Reload(MultiplicateurSpeedReload);
        }
    }

    public void Aim()
    {
        //if ()
        //{
        //    Debug.Log("ne peut pas viser");
        //    return;
        //}
        if (Input.GetKey(KeyCode.Mouse1) && (!playerCtrl.IsRunning && !currentWeapon.IsReloading))
        {
            IsAiming = true;
            camWeapon.fieldOfView = Mathf.Lerp(camWeapon.fieldOfView, currentWeapon.AimFov, currentWeapon.SpeedToScoop);
            weaponListT.transform.localPosition = Vector3.Lerp(weaponListT.transform.localPosition, aimPos.localPosition - difference, currentWeapon.SpeedToScoop);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            IsAiming = false;
        }
        else
        {
            camWeapon.fieldOfView = Mathf.Lerp(camWeapon.fieldOfView, FovDefault, currentWeapon.SpeedToScoop);
            weaponListT.transform.localPosition = Vector3.Lerp(weaponListT.transform.localPosition, defaultPos.localPosition, currentWeapon.SpeedToScoop);
        }
    }

    public void Cut()
    {

    }
    #endregion

    private void Moyenne(float _x, float _y)
    {
        //Debug.Log("x" + _x);
        //Debug.Log("y" + _y);
        X.Add(_x);
        Y.Add(_y);
        SommeX += _x;
        SommeY += _y;

        MoyX = SommeX / X.Count;
        MoyY = SommeY / Y.Count;
    }
}
