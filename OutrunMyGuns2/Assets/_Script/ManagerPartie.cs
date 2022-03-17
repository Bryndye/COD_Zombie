using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerPartie : MonoBehaviour
{
    public static ManagerPartie Instance;
    WaveManager waveManager;

    [Header("Stats Canvas")]
    [SerializeField] PlayerStats prefabPlayerStat;
    [SerializeField] GameObject canvasStats, parentPStats;
    public List<PlayerLife> Players;
    public List<PlayerStats> PlayersStats;
    public List<PlayerPoints> PlayersPoints;
    public List<PlayerWeapon> PlayersWeapon;

    [Header("Elements To Activate")]
    [SerializeField] AudioSource musicEnd;
    [SerializeField] GameObject cinematicEnd;
    [SerializeField] GameObject endGameText;
    [SerializeField] TextMeshProUGUI mancheSurvive;
    bool activate = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        waveManager = WaveManager.Instance;

        for (int i = 0; i < Players.Count; i++)
        {
            PlayersStats.Add(Instantiate(prefabPlayerStat, parentPStats.transform));
            PlayersStats[i].PlayerName.text = "Player " + i;
            PlayersPoints.Add(Players[i].GetComponent<PlayerPoints>());
            PlayersWeapon.Add(Players[i].GetComponent<PlayerWeapon>());
        }
    }

    void Update()
    {
        UpdateStatPlayersUI();
        DeathPlayers();

        if(!activate)
            canvasStats.SetActive(Input.GetKey(KeyCode.Tab));
    }

    private void UpdateStatPlayersUI()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            PlayersStats[i].PlayerName.text = "Player " + i;
            PlayersStats[i].Points.text = PlayersPoints[i].FinalPoints.ToString();
            PlayersStats[i].Kill.text = PlayersPoints[i].Kills.ToString();
            PlayersStats[i].Headsoots.text = PlayersPoints[i].HeadShoots.ToString();
        }
    }

    private void DeathPlayers()
    {
        if (activate)
        {
            return;
        }
        foreach (var player in Players)
        {
            if (!player.IsDead)
            {
                return;
            }
        }
        activate = true;
        Invoke(nameof(BackToMenu), 16);
        foreach (var player in Players)
        {
            player.gameObject.SetActive(false);
        }
        mancheSurvive.text = "Vous avez survécu " + (waveManager.Round - 1) + "!";
        cinematicEnd.SetActive(true);
        canvasStats.SetActive(true);
        musicEnd.Play();
        //Debug.Log("Anim cam");
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
