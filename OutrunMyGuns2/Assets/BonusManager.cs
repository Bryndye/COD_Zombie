using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusType { InstaKill, DoublePoints, MaxAmmo, Nuke}
public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance;
    ManagerPartie managerPartie;

    public List<PlayerUI> PlayersUI;
    public List<GameObject> Bonuses;

    public bool IsDoublePoints, IsInstaKill;
    //public bool HasInstaKill, HasDoublePoints, HasMaxAmmo, HasNuke;
    //Ajouter pour plus tard limitation de bonus max par round

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        managerPartie = ManagerPartie.Instance;
    }

    public void GetBonus(BonusType _bonus)
    {
        switch (_bonus)
        {
            case BonusType.InstaKill:
                InstaKillEffect();
                break;
            case BonusType.DoublePoints:
                DoublePointsEffect();
                break;
            case BonusType.MaxAmmo:
                MaxAmmoEffect();
                break;
            case BonusType.Nuke:
                NukeEffect();
                break;
            default:
                break;
        }
    }

    //ADD SOUND EFFECT ! + SPAWN RNG BONUS

    #region Bonus effects / ui feedback
    private void InstaKillEffect()
    {
        IsInstaKill = true;
        CancelInvoke(nameof(StopInstaKill));
        foreach (var playerUI in PlayersUI)
        {
            playerUI.InstaKillUI.SetActive(true);
            playerUI.Effect("Insta Kill !");
        }
        Invoke(nameof(StopInstaKill), 30);
        //CancelInvoke();
    }

    private void StopInstaKill()
    {
        IsInstaKill = false;
        foreach (var playerUI in PlayersUI)
        {
            playerUI.InstaKillUI.SetActive(false);
        }
    }

    private void DoublePointsEffect()
    {
        IsDoublePoints = true;
        CancelInvoke(nameof(StopDoublePoints));
        foreach (var playerUI in PlayersUI)
        {
            playerUI.DoublePointsUI.SetActive(true);
            playerUI.Effect("Double Points !");
        }
        Invoke(nameof(StopDoublePoints), 30);
    }

    private void StopDoublePoints()
    {
        IsDoublePoints = false;
        foreach (var playerUI in PlayersUI)
        {
            playerUI.DoublePointsUI.SetActive(false);
        }
    }

    private void MaxAmmoEffect()
    {
        foreach (var playerWeapon in managerPartie.PlayersWeapon)
        {
            foreach (var weapon in playerWeapon.MyWeapons)
            {
                weapon.MaxAmmo();
            }
        }
        foreach (var playerUI in PlayersUI)
        {
            playerUI.Effect("Max Ammo !");
        }
    }

    private void NukeEffect()
    {
        foreach (var playerUI in PlayersUI)
        {
            playerUI.Effect("Kaboom !");
        }
        Debug.Log("nuke EFFECT");
    }
    #endregion

    public void SpawnBonus(Vector3 _pos)
    {
        float _rng = Random.Range(0,1000);
        if (_rng > 100)
        {
            return;
        }
        Instantiate(Bonuses[Random.Range(0, Bonuses.Count)], _pos + Vector3.up, Quaternion.identity);
    }
}
