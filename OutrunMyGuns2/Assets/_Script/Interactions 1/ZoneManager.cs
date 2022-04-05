using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    WaveManager waveManager;

    public List<Door> Doors;
    public List<SpawnZombie> SpawnZombies;

    private void Start()
    {
        waveManager = WaveManager.Instance;
    }


    public void DoorOpenEvent()
    {
        //Debug.Log(gameObject.name + " is openned !");

        waveManager.Spawns.AddRange(SpawnZombies);
        foreach (var _door in Doors)
        {
            _door.RemoveTheDoor();
        }
    }
}
