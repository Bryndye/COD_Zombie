using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombie : MonoBehaviour
{
    WaveManager waveMan;

    public Transform AssociateWindow;
    public bool canSpawn = false;
    public bool nearest = false;

    public float ScoreFinal;
    float score;
    //Prendre les spawns les plus proches du joueur donc les 6(var edit) premiers spawns

    private void Start()
    {
        waveMan = WaveManager.Instance;
    }

    void Update()
    {
        CheckScoreForAllPlayers();
    }

    private void CheckScoreForAllPlayers()
    {
        if (waveMan == null)
        {
            return;
        }
        score = 100000;
        foreach (var item in waveMan.Players)
        {
            float _scorePlayer = Vector3.Distance(transform.position, item.transform.position);
            score = _scorePlayer < score ? _scorePlayer : ScoreFinal;
        }

        ScoreFinal = score;
        score = 0;
    }

    public void InstantiateZombies()
    {
        //var _zombie = Instantiate(waveMan.prefabZombie, transform.position, transform.rotation);
        //waveMan.CurrentZombies.Add(_zombie);
        //_zombie.Life = waveMan.GetHealthZombieRound();
        //_zombie.Target = waveMan.Players[0].transform;

        waveMan.ZombiesPerRound--;
    }
}
