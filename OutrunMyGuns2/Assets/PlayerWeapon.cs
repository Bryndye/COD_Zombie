using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Mes Armes")]
    [SerializeField] Transform camTransform;
    public List<Weapon> MyWeapons;
    [SerializeField] Weapon currentWeapon;

    void Start()
    {
        currentWeapon = MyWeapons.FirstOrDefault();
    }

    void Update()
    {
        
    }


    #region Input Handler
    public void Scroll(InputAction.CallbackContext ctx)
    {
        int _index = MyWeapons.IndexOf(currentWeapon) + 1;
        if (_index >= MyWeapons.Count)
        {
            _index = 0;
        }
        currentWeapon = MyWeapons[_index];
    }

    public void Fire(InputAction.CallbackContext ctx)
    {
        if (!currentWeapon.CanShoot)
            return;

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

    public void Reload(InputAction.CallbackContext ctx)
    {

    }

    public void Aim(InputAction.CallbackContext ctx)
    {

    }

    public void Cut(InputAction.CallbackContext ctx)
    {

    }
    #endregion
}
