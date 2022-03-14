using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public List<PlayerLife> Players;
    public List<SpawnZombie> Spawns;

    [Header("Rounds")]
    public int Round = 0;
    public int ZombiesPerRound = 0, ZombiesLeft = 0;
    [SerializeField] AudioSource audioNewRound;
    [SerializeField] AudioClip[] audios;
    bool endRound = false;

    [Header("PoolSystem Zombies")]
    public ZombieBehaviour prefabZombie;
    [SerializeField] int numberOfZombiesToPool;
    [SerializeField] Transform parentZombiesToPool;
    public List<ZombieBehaviour> ZombiesPool;
    int indexZombies = 0;

    [Header("param spawns")]
    public List<ZombieBehaviour> CurrentZombies;
    [SerializeField] int zombiesInRoomMax { get { return 18 + 6 * Players.Count; } }
    [SerializeField] float timeBetweenSpawn = 1f;
    float timeToSpawn;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI roundT;
    [SerializeField] Animator roundAnim;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InstantiateAllZombies();
        NewRound();
    }

    private void InstantiateAllZombies()
    {
        for (int i = 0; i < numberOfZombiesToPool; i++)
        {
            var _zb = Instantiate(prefabZombie, parentZombiesToPool);
            _zb.gameObject.SetActive(false);
            ZombiesPool.Add(_zb);
        }
    }

    public void PoolAZombieToSpawn(SpawnZombie _spawn)
    {
        indexZombies++;
        if (indexZombies >= numberOfZombiesToPool)
        {
            indexZombies = 0;
        }
        var _zb = ZombiesPool[indexZombies];

        _zb.transform.position = _spawn.transform.position;
        _zb.gameObject.SetActive(true);

        CurrentZombies.Add(_zb);
        _zb.Life = GetHealthZombieRound();
        _zb.myNormalStates = GetSpeedZombieRound();
        _zb.ChooseMySpeed();
        _zb.WindowTarget = _spawn.AssociateWindow;
        _zb.Target = Players[0].transform;

        _zb.Reviving();

        ZombiesPerRound--;
    }

    void Update()
    {
        roundT.text = Round.ToString();

        if (ZombiesLeft <= 0 && !endRound)
        {
            EndRound();
        }

        if (ZombiesPerRound <= 0)
        {
            return;
        }
        if (CurrentZombies.Count >= zombiesInRoomMax)
        {
            return;
        }
        timeToSpawn += Time.deltaTime;
        if (timeToSpawn >= timeBetweenSpawn)
        {
            SpawnZombie _spawn = Spawns[Random.Range(0, Spawns.Count)];
            PoolAZombieToSpawn(_spawn);
            //Spawns[Random.Range(0, Spawns.Count)].InstantiateZombies();
            //Spawn zombies sur les spawns parmis les CanSpawn && Nearest puis random entre tous ceux la
            //Plus on avance dans les rounds, plus les zombies spawn vite 

            //Spawn a zombie

            timeToSpawn = 0;
        }
    }

    public int GetHealthZombieRound()
    {
        if (Round < 10)
        {
            return 100 * Round + 50;
        }
        else
        {
            return Mathf.RoundToInt(950 * Mathf.Pow(1.1f, Round - 9));
        }
    }

    private ZombieStates GetSpeedZombieRound()
    {
        if (Round >= 7)
        {
            Debug.Log("RUN !");
            return ZombieStates.Run;
        }
        else
        {
            return ZombieStates.Walk;
        }
    }

    public void RemoveZombie(ZombieBehaviour _zb)
    {
        CurrentZombies.Remove(_zb);
        ZombiesLeft--;
    }

    private void EndRound()
    {
        endRound = true;
        roundAnim.SetTrigger("BeforeWave");
        audioNewRound.clip = audios[0];
        audioNewRound.Play();
        Invoke(nameof(NewRound), 8f);
    }

    private void NewRound()
    {
        endRound = false;
        roundAnim.SetTrigger("NewWave");
        Round++;
        audioNewRound.Stop();
        audioNewRound.clip = audios[1];
        audioNewRound.Play();
        ZombiesPerRound = Mathf.RoundToInt((float)(0.000058 * Mathf.Pow(Round, 3) + 0.074032 * Mathf.Pow(Round, 2) + 0.718119 * Round + 14.38699));
        ZombiesLeft = ZombiesPerRound;
    }
}
