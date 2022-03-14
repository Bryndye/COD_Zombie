using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudioManager : MonoBehaviour
{
    public AudioSource MyAudioSource;
    public List<AudioClip> AmbientSounds, AttackSounds, RunSounds, DeathSounds;

    //settrigger if attack, death
    // -> pour la mort, une chance sur X de lancer le son ? verifier si il y a peu de zombies ?
    //si ambient ou run toutes les X secondes Play();
}
