using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<PlayerLife> Players;
    public List<SpawnZombie> Spawns;

    [Header("Rounds")]
    public int Round = 0;
    public int ZombiesPerRound = 0;
    [SerializeField] AudioSource audioNewRound;

    [Header("param spawns")]
    [SerializeField] ZombieBehaviour prefabZombie;
    public List<ZombieBehaviour> CurrentZombies;
    [SerializeField] int zombiesInRoomMax = 24;
    [SerializeField] float timeBetweenSpawn = 1f;
    float timeToSpawn;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI roundT;

    private void Start()
    {
        NewRound();
    }

    void Update()
    {
        roundT.text = Round.ToString();

        if (CurrentZombies.Count >= zombiesInRoomMax)
        {
            return;
        }
        timeToSpawn += Time.deltaTime;
        if (timeToSpawn >= timeBetweenSpawn)
        {
            int _int = Random.Range(0, Spawns.Count);
            CurrentZombies.Add(Instantiate(prefabZombie, Spawns[_int].transform.position, Spawns[_int].transform.rotation));
            CurrentZombies.LastOrDefault().Target = Players[0].transform;
            GetHealthZombieRound(CurrentZombies.LastOrDefault());
            //Spawn a zombie
        }
    }

    private void GetHealthZombieRound(ZombieBehaviour _zb)
    {
        if (Round < 10 )
        {
            _zb.Life = 100 * Round + 50;
        }
        else
        {
            _zb.Life = Mathf.RoundToInt(950 * Mathf.Pow(1.1f, Round - 9));
        }
    }

    private void NewRound()
    {
        Round++;
        audioNewRound.Play();
        ZombiesPerRound = Mathf.RoundToInt((float)(0.000058 * Mathf.Pow(Round, 3) + 0.074032 * Mathf.Pow(Round, 2) + 0.718119 * Round + 14.38699));
    }
}
