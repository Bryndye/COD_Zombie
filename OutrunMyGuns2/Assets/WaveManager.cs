using System.Collections;
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
        if (CurrentZombies.Count >= zombiesInRoomMax)
        {
            return;
        }
        timeToSpawn += Time.deltaTime;
        if (timeToSpawn >= timeBetweenSpawn)
        {
            //Spawn a zombie
        }
    }

    private void NewRound()
    {
        Round++;
        audioNewRound.Play();
        ZombiesPerRound = Mathf.RoundToInt((float)(0.000058 * Mathf.Pow(Round, 3) + 0.074032 * Mathf.Pow(Round, 2) + 0.718119 * Round + 14.38699));
    }
}
