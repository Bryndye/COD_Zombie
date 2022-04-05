using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudioManager : MonoBehaviour
{
    ZombieBehaviour zombieBehaviour;
    public AudioSource MyAudioSource;
    public List<AudioClip> AmbientSounds, AttackSounds, RunSounds, DeathSounds;

    public float TimeMaxBetweenScream = 3f;
    float timeToScream, timeTempScream;
    //settrigger if attack, death
    // -> pour la mort, une chance sur X de lancer le son ? verifier si il y a peu de zombies ?
    //si ambient ou run toutes les X secondes Play();

    private void Awake()
    {
        zombieBehaviour = GetComponent<ZombieBehaviour>();
    }

    private void Start()
    {
        timeToScream = Random.Range(5, TimeMaxBetweenScream);
    }

    private void Update()
    {
        if (MyAudioSource.isPlaying || zombieBehaviour.IsDead)
        {
            return;
        }
        timeTempScream += Time.deltaTime;
        if (timeTempScream >= timeToScream)
        {
            timeTempScream = 0;
            if (zombieBehaviour.myNormalStates == ZombieStates.Run)
            {
                int _rng = Random.Range(0, RunSounds.Count);
                MyAudioSource.clip = RunSounds[_rng];
            }
            else
            {
                int _rng = Random.Range(0, AmbientSounds.Count);
                MyAudioSource.clip = AmbientSounds[_rng];
            }
            MyAudioSource.Play();
            timeToScream = Random.Range(3, TimeMaxBetweenScream);
        }
    }

    public void SoundAttack()
    {
        int _rng = Random.Range(0, AttackSounds.Count);
        MyAudioSource.clip = AttackSounds[_rng];
        MyAudioSource.Play();
    }

    public void SoundDeath()
    {
        int _rng = Random.Range(0, DeathSounds.Count);
        MyAudioSource.clip = DeathSounds[_rng];
        MyAudioSource.Play();
    }
}
