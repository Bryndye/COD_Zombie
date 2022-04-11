using UnityEngine;
using UnityEngine.UI;

public enum Perks { Mastodonte, PassePass, DoubleTap, ThreeW, Revive };
public class PerksPlayer : MonoBehaviour
{
    PlayerLife playerLife;
    PlayerWeapon playerWeapon;

    [Header("MASTO")]
    public bool HasMasto = false;
    public int LifeMax = 250;
    public Image IconM;

    [Header("Passe Passe")]
    public bool HasPassPass = false;
    public float SpeedMultiReload = 2;
    public Image IconPP;

    [Header("Double Tap")]
    public bool HasDoubleTap = false;
    public int MuliplicateurBullets = 2;
    public Image IconDP;

    [Header("Revive")]
    public bool HasRevive = false;
    public int CountOfAutoReive = 3;
    public Image IconRev;

    [Header("ThreeW")]
    public bool HasThreeW = false;
    public int CountWeapons = 3;
    public Image IconThreeW;

    private void Awake()
    {
        playerLife = GetComponent<PlayerLife>();
        playerWeapon = GetComponent<PlayerWeapon>();
    }

    private void Start()
    {
        IconM.gameObject.SetActive(HasMasto);
        IconPP.gameObject.SetActive(HasPassPass);
        IconDP.gameObject.SetActive(HasDoubleTap);
        IconRev.gameObject.SetActive(HasRevive);
        IconThreeW.gameObject.SetActive(HasThreeW);
    }

    #region GetStats from perks
    public void GetMasto()
    {
        HasMasto = true;
        playerLife.LifeMax = LifeMax;
        playerLife.Life = playerLife.LifeMax;
        IconM.gameObject.SetActive(HasMasto);
        IconM.transform.SetSiblingIndex(0);
    }

    public void GetPassPass()
    {
        HasPassPass = true;
        playerWeapon.MultiplicateurSpeedReload = SpeedMultiReload;
        IconPP.gameObject.SetActive(HasPassPass);
        IconPP.transform.SetSiblingIndex(0);
    }

    public void GetDoubleTap()
    {
        HasDoubleTap = true;
        playerWeapon.MultiplicateurDamage = MuliplicateurBullets;
        IconDP.gameObject.SetActive(HasDoubleTap);
        IconDP.transform.SetSiblingIndex(0);
    }

    public void GetRevive()
    {
        HasRevive = true;
        //Do something
        IconRev.gameObject.SetActive(HasRevive);
        IconRev.transform.SetSiblingIndex(0);
    }

    public void GetThreeW()
    {
        HasThreeW = true;
        playerWeapon.CountMaxWeapons = CountWeapons;
        IconThreeW.gameObject.SetActive(HasThreeW);
        IconThreeW.transform.SetSiblingIndex(0);
    }
    #endregion


    #region Check For perks
    public void WhichPerkToBoy(Perks _perk)
    {
        //Debug.Log("perk en cours");

        switch (_perk)
        {
            case Perks.Mastodonte:
                GetMasto();
                break;
            case Perks.PassePass:
                GetPassPass();
                break;
            case Perks.DoubleTap:
                GetDoubleTap();
                break;
            case Perks.ThreeW:
                GetThreeW();
                break;
            case Perks.Revive:
                GetRevive();
                break;
            default:
                break;
        }
    }

    public bool AlreadyHas(Perks _perk)
    {
        switch (_perk)
        {
            case Perks.Mastodonte:
                return HasMasto;
            case Perks.PassePass:
                return HasPassPass;
            case Perks.DoubleTap:
                return HasDoubleTap;
            case Perks.ThreeW:
                return HasThreeW;
            case Perks.Revive:
                return HasRevive;

            default:
                return false;
        }
    }

    #endregion
}
