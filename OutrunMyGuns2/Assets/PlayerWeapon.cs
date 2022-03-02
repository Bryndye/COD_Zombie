using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Pos weapons")]
    [SerializeField] Transform defaultPos, aimPos;

    [Header("Mes Armes")]
    [SerializeField] Transform camTransform;
    [SerializeField] Animator animHands;
    public List<Weapon> MyWeapons;
    [SerializeField] Weapon currentWeapon;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI WeaponNameT, MunChargT, MunStockT;

    void Start()
    {
        currentWeapon = MyWeapons.FirstOrDefault();
    }

    void Update()
    {
        InputManager();
        UI();
    }

    void InputManager()
    {
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            Fire();
        }
        if (Input.GetAxisRaw("Reload") != 0)
        {
            Reload();
        }
        if (Input.GetAxisRaw("Scroll") > 0)
        {
            Scroll();
        }
        //Aim();
        if (Input.GetAxisRaw("Cut") != 0)
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

        currentWeapon.MunitionChargeur--;
        for (int i = 0; i < currentWeapon.BulletsPerShoot; i++)
        {
            float _rng = Random.Range(-currentWeapon.Precision, currentWeapon.Precision);
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit))
            {
                Debug.Log(hit.collider);
                //hit.collider.TryGetComponent(out );
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
        if (Input.GetAxisRaw("Aim") > 0)
        {
            Vector3.Lerp(currentWeapon.transform.localPosition, aimPos.position, 1);
        }
        else
        {
            Vector3.Lerp(currentWeapon.transform.localPosition, defaultPos.position, 1);
        }
    }

    public void Cut()
    {

    }
    #endregion
}
