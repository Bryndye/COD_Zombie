using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    StressReceiver camShake;

    [Header("Pos weapons")]
    [SerializeField] Transform defaultPos, aimPos;
    [SerializeField] Transform weaponListT;

    [Header("Mes Armes")]
    [SerializeField] Transform camTransform;
    [SerializeField] Animator animHands;
    public List<Weapon> MyWeapons;
    [SerializeField] Weapon currentWeapon;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI WeaponNameT, MunChargT, MunStockT;

    private void Awake()
    {
        camShake = GetComponentInChildren<StressReceiver>();
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


    #region Input Handler
    public void Scroll()
    {
        Debug.Log("bo");
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
        if (!currentWeapon.CanShoot)
            return;

        camShake.InduceStress(0.05f); //pas sur de garder le shake !
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
        if (currentWeapon.MunitionsStock > 0 && !currentWeapon.isReloading)
        {
            currentWeapon.Reload();
        }
    }

    public void Aim()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Vector3.Lerp(currentWeapon.transform.localPosition, aimPos.localPosition, 0.01f);
        }
        else
        {
            Vector3.Lerp(currentWeapon.transform.localPosition, defaultPos.localPosition, 0.01f);
        }
    }

    public void Cut()
    {

    }
    #endregion
}
