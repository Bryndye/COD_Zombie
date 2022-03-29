using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    PlayerController pc;
    PlayerWeapon pw;
    PerksPlayer perksP;
    PlayerPoints pPoints;

    [SerializeField] float distanceMaxInteraction;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] TextMeshProUGUI interactText;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
        pw = GetComponent<PlayerWeapon>();
        perksP = GetComponent<PerksPlayer>();
        pPoints = GetComponent<PlayerPoints>();
    }

    void Update()
    {
        CheckFront();
    }

    private void CheckFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(pc.cameraTransform.position, pc.cameraTransform.forward, out hit, distanceMaxInteraction, ~ignoreLayer))
        {
            if (hit.collider.TryGetComponent(out PerkBoitier _pkb) && !perksP.AlreadyHas(_pkb.MyPerk))
            {
                BuyPerk(_pkb);
            }
            else if (hit.collider.TryGetComponent(out Window _w))
            {
                if (_w.Full || !_w.CanRebuild)
                {
                    interactText.text = "";
                    return;
                }
                RepairWindow(_w);
            }
            else if (hit.collider.TryGetComponent(out WeaponWall _ww))
            {
                BuyWeapon(_ww);
            }
            else if (hit.collider.TryGetComponent(out ElementInteractable _element))
            {
                InterectElements(_element);
            }
            else if (hit.collider.TryGetComponent(out Door _door))
            {
                BuyDoor(_door);
            }
            else
            {
                //Debug.Log(hit.collider.name);
                interactText.text = "";
            }
        }
        else
        {
            interactText.text = "";
        }
    }

    #region Fct for each interactions

    private void BuyPerk(PerkBoitier _pkb)
    {
        interactText.text = "Press E to buy " + _pkb.MyPerk.ToString() + " for " + _pkb.Cost;

        if (Input.GetKeyDown(KeyCode.E) && pPoints.CanPlayerBuyIt(_pkb.Cost))
        {
            perksP.WhichPerkToBoy(_pkb.MyPerk);
            pPoints.Buy(_pkb.Cost);
        }
    }

    private void RepairWindow(Window _w)
    {
        interactText.text = "Hold E to rebuild";

        if (Input.GetKey(KeyCode.E))
        {
            _w.Rebuild(pPoints);
        }
    }

    private void BuyWeapon(WeaponWall _ww)
    {
        bool _hasThis = false;
        foreach (var weapon in pw.MyWeapons)
        {
            if (weapon.Name == _ww.PrefabWeapon.Name)
            {
                _hasThis = true;
                Debug.Log("cant buy");
                break;
            }
        }
        if (_hasThis)
        {
            interactText.text = "Press E to buy " + _ww.PrefabWeapon.Name + " ammo for [" + _ww.AmmoCost + "]";

            if (Input.GetKeyDown(KeyCode.E) && pPoints.CanPlayerBuyIt(_ww.AmmoCost))
            {
                pPoints.Buy(_ww.AmmoCost);
                pw.currentWeapon.MaxAmmo();
            }
        }
        else
        {
            interactText.text = "Press E to buy " + _ww.PrefabWeapon.Name + " for [" + _ww.WeaponCost + "]";
            if (Input.GetKeyDown(KeyCode.E) && pPoints.CanPlayerBuyIt(_ww.WeaponCost))
            {
                pPoints.Buy(_ww.WeaponCost);
                pw.TakeWeapon(_ww.PrefabWeapon);
            }
        }
    }

    private void BuyDoor(Door _door)
    {
        interactText.text = "Press E to buy the door for " + _door.Cost;

        if (Input.GetKeyDown(KeyCode.E) && pPoints.CanPlayerBuyIt(_door.Cost))
        {
            pPoints.Buy(_door.Cost);
            _door.OpenTheDoor.Invoke();
        }
    }

    private void InterectElements(ElementInteractable _element)
    {
        if (!_element.isShootable && !_element.isShootable && Input.GetKeyDown(KeyCode.E))
        {
            _element.ActivateElement();
        }
    }

    #endregion
}
