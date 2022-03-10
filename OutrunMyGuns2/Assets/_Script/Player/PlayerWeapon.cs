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
    public bool IS_INSTANTIATE_DEBUG_SHOOT = true;
    public GameObject PREFAB_TEST_SHOOT;

    [Header("Scope")]
    public float FovDefault;
    [SerializeField] Camera camWeapon;
    Vector3 difference { get { return new Vector3(0, currentWeapon.AimPos.localPosition.y, 0); } }
    float coefTotal = 1, coefAim = 1, coefMovement = 1, coefShooting = 1, coefCrouch = 1;

    [Header("Perks Effets")]
    public float MultiplicateurBullets = 1;
    public float MultiplicateurSpeedReload = 1;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI WeaponNameT;
    [SerializeField] TextMeshProUGUI MunChargT;
    [SerializeField] TextMeshProUGUI MunStockT;
    [SerializeField] TextMeshProUGUI COEFPRECISION;
    [SerializeField] GameObject Hitmarker, Reticules;
    [SerializeField] RectTransform[] ReticulesT;

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
        ModificateurAim();
        GetPrecision();
        InputManager();
        UI();
    }

    void InputManager()
    {
        if (currentWeapon.canHold && Input.GetAxisRaw("Fire1") != 0)
        {
            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
        else
        {
            coefShooting = 1;
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
        coefAim = IsAiming ? 0.1f : 1;
        coefCrouch = playerCtrl.IsCrouching ? 0.5f : 1;
        switch (playerCtrl.MovementState)
        {
            case MovementState.Idle:
                coefMovement = 1;
                break;
            case MovementState.Walk:
                coefMovement = 2f;
                break;
            case MovementState.Run:
                coefMovement = 3f;
                break;
            default:
                break;
        }
        coefTotal = coefAim * coefMovement * coefShooting * coefCrouch;
        COEFPRECISION.text = coefTotal.ToString();
    }

    #region UI - feedback
    private void UI()
    {
        WeaponNameT.text = currentWeapon.Name;
        MunChargT.text = currentWeapon.MunitionChargeur.ToString();
        MunStockT.text = currentWeapon.MunitionsStock.ToString();

        Reticules.SetActive(!IsAiming && !playerCtrl.IsRunning);
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


    #endregion

    #region Precision + Reticules
    float distance, ration;
    private void GetPrecision()
    {
        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit))
        {
            distance = Vector3.Distance(camTransform.position, hit.point);
            //Debug.Log(distance);

            ration = Mathf.Atan(currentWeapon.Precision / 5) * coefTotal;
            UpdatePosReticules();
            //Debug.Log(distance * Mathf.Tan(alpha));
        }
    }

    private void UpdatePosReticules()
    {
        // x20 pour la position des reticules
        float _prec = Mathf.Lerp(ReticulesT[0].localPosition.y, currentWeapon.Precision * 200 * coefTotal, 0.5f);
        ReticulesT[0].localPosition = new Vector3(0, _prec, 0);
        ReticulesT[1].localPosition = new Vector3(0, -_prec, 0);
        ReticulesT[2].localPosition = new Vector3(_prec, 0, 0);
        ReticulesT[3].localPosition = new Vector3(-_prec, 0, 0);
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
    }

    public void Fire()
    {
        if (!currentWeapon.CanShoot || playerCtrl.MovementState == MovementState.Run)
            return;

        camRecoil.RecoilFire();
        coefShooting = 2;

        currentWeapon.MunitionChargeur--;
        if (currentWeapon.SFX != null)
            currentWeapon.SFX.Play();

        for (int i = 0; i < currentWeapon.BulletsPerShoot * MultiplicateurBullets; i++)
        {
            Vector3 _posInit = Vector3.zero;

            
            _posInit = Random.insideUnitSphere * distance * Mathf.Tan(ration);

            //_direction = Quaternion.Euler(0, indexLOCAL, 0) * _direction;

            RaycastHit[] hits = Physics.RaycastAll(camTransform.position + _posInit, camTransform.forward, 100, ~ignoreLayerShoot);
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
                    if (IS_INSTANTIATE_DEBUG_SHOOT)
                        Instantiate(PREFAB_TEST_SHOOT, hits[y].point, Quaternion.identity);

                }
            }

            //Debug.DrawRay(camTransform.position, _pos * 10, Color.green, 1);
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
}
