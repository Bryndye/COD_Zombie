using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    PlayerController playerCtrl;
    PlayerPoints playerPoints;
    PlayerCut playerCut;

    [Header("Pos weapons")]
    public bool IsAiming = false;
    [SerializeField] Transform defaultPos, aimPos;
    [SerializeField] Transform weaponListT;
    [SerializeField] CameraRecoil camRecoil;
    [SerializeField] Transform camTransform, audioParent;
    [HideInInspector] public Transform CamTransfrom { get {return camTransform; } }


    [Header("Mes Armes")]
    [SerializeField] LayerMask ignoreLayerShoot;
    public Animator AnimHands, AnimCam;
    public List<Weapon> MyWeapons;
    public Weapon currentWeapon;
    public int CountMaxWeapons;
    public bool IsReloading { get { return currentWeapon.IsReloading; } }

    [Header("Scope")]
    public float FovDefault;
    [SerializeField] Camera camWeapon;
    Vector3 difference { get { return new Vector3(0, currentWeapon.AimPos.localPosition.y, 0); } }
    float coefTotal = 1, coefAim = 1, coefMovement = 1, coefShooting = 1, coefCrouch = 1;
    public Vector3 VECTORCAMTEST;

    [Header("Perks Effets")]
    public float MultiplicateurBullets = 1;
    public float MultiplicateurSpeedReload = 1;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI WeaponNameT;
    [SerializeField] TextMeshProUGUI MunChargT;
    [SerializeField] TextMeshProUGUI MunStockT;
    [SerializeField] TextMeshProUGUI ReloadT;
    [SerializeField] TextMeshProUGUI COEFPRECISION;
    [SerializeField] GameObject Hitmarker, Reticules;
    [SerializeField] RectTransform[] ReticulesT;

    [Header("Debug")]
    public bool IS_INSTANTIATE_DEBUG_SHOOT = true;
    public GameObject PREFAB_TEST_SHOOT;

    private void Awake()
    {
        playerCtrl = GetComponent<PlayerController>();
        playerPoints = GetComponent<PlayerPoints>();
        playerCut = GetComponent<PlayerCut>();
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
        Aim();

        if (playerCut.IsCutting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.A) && MyWeapons.Count > 1)
        {
            SwitchWeaponUp();
        }

        if (currentWeapon.canHold && Input.GetAxisRaw("Fire1") != 0)
        {
            if (currentWeapon.IsReloading)
            {
                CancelAnimReload();
            }
            else
            {
                Fire();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentWeapon.IsReloading)
            {
                CancelAnimReload();
            }
            else
            {
                Fire();
            }
        }
        else
        {
            coefShooting = 1;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void ModificateurAim()
    {
        coefAim = IsAiming ? 0.1f : 1;
        coefCrouch = playerCtrl.IsCrouching ? 0.5f : 1;
        switch (playerCtrl.PlayerMvmtState)
        {
            case MovementState.Idle:
                AnimHands.SetBool("Run", false);
                coefMovement = 1;
                break;
            case MovementState.Walk:
                AnimHands.SetBool("Run", false);
                coefMovement = 1.5f;
                break;
            case MovementState.Run:
                AnimHands.SetBool("Run", true);
                coefMovement = 2f;
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
        MunChargT.text = currentWeapon.AmmoMag.ToString();
        MunChargT.color = currentWeapon.AmmoMag < currentWeapon.AmmoMaxMag * 0.3f ? Color.red : Color.white;
        MunStockT.text = "/ " + currentWeapon.AmmoStock.ToString();
        MunStockT.color = currentWeapon.AmmoStock < currentWeapon.AmmoMaxMag * 2 ? Color.red : Color.white;

        ReloadText();
        Reticules.SetActive(!IsAiming && !playerCut.IsCutting && !currentWeapon.IsReloading);
    }

    private void ReloadText()
    {
        ReloadT.gameObject.SetActive(currentWeapon.AmmoMag < currentWeapon.AmmoMaxMag * 0.3f || currentWeapon.IsReloading);
        if (ReloadT.IsActive())
        {
            if (currentWeapon.AmmoMag == 0 && currentWeapon.AmmoStock == 0)
            {
                ReloadT.GetComponent<Animator>().SetBool("HasAmmo", false);
                ReloadT.text = "No ammo";
            }
            else if (currentWeapon.IsReloading)
            {
                ReloadT.GetComponent<Animator>().SetBool("HasAmmo", true);
                ReloadT.text = "Reloading...";
            }
            else if(currentWeapon.AmmoMag < currentWeapon.AmmoMaxMag * 0.3f && currentWeapon.AmmoStock > 0)
            {
                ReloadT.GetComponent<Animator>().SetBool("HasAmmo", true);
                ReloadT.text = "Press R to reload";
            }
            else
            {
                ReloadT.text = "";
            }
        }
    }


    private void HitmarkerActiveUI()
    {
        Hitmarker.SetActive(false);
        Hitmarker.SetActive(true);
    }

    public void FeedbackHit(int _p, bool _kill = false, bool _head = false)
    {
        HitmarkerActiveUI();
        playerPoints.GetPoints(_p);
        if(_kill)
            playerPoints.GetStats(_head);
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

    public void Fire()
    {
        if (!currentWeapon.CanShoot || playerCtrl.PlayerMvmtState == MovementState.Run)
            return;

        camRecoil.RecoilFire();
        coefShooting = 2;

        currentWeapon.AmmoMag--;
        PoolSystem.Instance.SetSfx(currentWeapon.ClipShoot, audioParent);

        for (int i = 0; i < currentWeapon.BulletsPerShoot * MultiplicateurBullets; i++)
        {
            Vector3 _posInit = Vector3.zero;
            Vector3 _direction = camTransform.forward;

            _posInit = Random.insideUnitSphere * distance * Mathf.Tan(ration);
            RaycastHit _hit;
            if (Physics.Raycast(camTransform.position + _posInit, camTransform.forward, out _hit, 300, ~ignoreLayerShoot))
            {
                _direction = _hit.point - camTransform.position;
            }

            RaycastHit[] hits = Physics.RaycastAll(camTransform.position, _direction, 300, ~ignoreLayerShoot);
            for (int y = 0; y < hits.Length; y++)
            {
                if (hits[y].collider.TryGetComponent(out ZombieBehaviour _zb))
                {
                    _zb.TakeDamage(currentWeapon.Damage, this);
                }
                else if (hits[y].collider.TryGetComponent(out PartOfBody _head))
                {
                    _head.TakeDamage(currentWeapon.Damage, this, TypeKill.Head);
                }
                else if (hits[y].collider.TryGetComponent(out ElementInteractable _int))
                {
                    _int.ActivateElement();
                }
                else
                {
                    if (IS_INSTANTIATE_DEBUG_SHOOT)
                        Instantiate(PREFAB_TEST_SHOOT, hits[y].point, Quaternion.identity);

                }
                Debug.DrawRay(camTransform.position, _direction * 10, Color.green, 1);
            }

        }
        currentWeapon.CanShoot = false;
        currentWeapon.TimeToShoot = 0;
    }
    #endregion

    #region Input Handler
    public void SwitchWeaponUp()
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

    public void Reload()
    {
        if (currentWeapon.IsReloading)
        {
            return;
        }
        if (currentWeapon.AmmoStock > 0 && currentWeapon.AmmoMag != currentWeapon.AmmoMaxMag  && !currentWeapon.IsReloading)
        {
            currentWeapon.Reload(MultiplicateurSpeedReload);
        }
    }

    public void Aim()
    {
        if (Input.GetKey(KeyCode.Mouse1) && playerCtrl.PlayerMvmtState != MovementState.Run && !currentWeapon.IsReloading && !playerCut.IsCutting)
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
    #endregion

    public void TakeWeapon(Weapon _weapon)
    {
        if (MyWeapons.Count >= CountMaxWeapons)
        {
            //current weapon a enlever de la list puis destroy
            var _currentWDelete = currentWeapon;
            MyWeapons.Remove(_currentWDelete);
            Destroy(_currentWDelete.gameObject);
        }
        //instantiate nouvelle arme
        //add to list Weapons
        //active cette arme
        var _futurWeapon = Instantiate(_weapon, weaponListT);
        MyWeapons.Add(_futurWeapon);
        foreach (var weapon in MyWeapons)
        {
            weapon.gameObject.SetActive(false);
        }
        currentWeapon = _futurWeapon;
        currentWeapon.gameObject.SetActive(true);
        //Debug.Log("I switch or get this weapon " + _weapon.Name);
    }

    public void CancelAnimReload()
    {
        currentWeapon.CancelInvoke();
        currentWeapon.IsReloading = false;
    }
}
