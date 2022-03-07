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
        IconM.gameObject.SetActive(HasMasto);
    }

    public void GetPassPass()
    {
        HasPassPass = true;
        playerWeapon.MultiplicateurSpeedReload = SpeedMultiReload;
        IconPP.gameObject.SetActive(HasPassPass);
    }

    public void GetDoubleTap()
    {
        HasDoubleTap = true;
        playerWeapon.MultiplicateurBullets = MuliplicateurBullets;
        IconDP.gameObject.SetActive(HasDoubleTap);
    }

    public void GetRevive()
    {
        HasRevive = true;
        //Do something
        IconRev.gameObject.SetActive(HasRevive);
    }

    public void GetThreeW()
    {
        HasThreeW = true;
        playerWeapon.CountMaxWeapons = CountWeapons;
        IconThreeW.gameObject.SetActive(HasThreeW);
    }
    #endregion


    #region Check For perks
    public void WhichPerkToBoy(Perks _perk)
    {
        Debug.Log("perk en cours");

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

    public void CanPlayerBuyIt(Perks _perkToBuy)
    {
        //if player has enough points, can buy it
        //if player has already it, dont boy it

    }

    #endregion
}
