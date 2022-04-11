using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMenuAnim : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] Vector3 PosInit, FinalPos;
    bool canMove;

    public bool CanPlaySound;
    AudioSource audioS;
    [SerializeField] AudioClip[] AmbientSound;
    float time;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
        transform.position = PosInit;
        transform.LookAt(FinalPos);
        Invoke(nameof(canMovefct), Random.Range(0,2));
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, FinalPos, Speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, FinalPos) < 0.1f)
        {
            transform.LookAt(FinalPos);
            transform.position = PosInit;
        }

        if (!CanPlaySound)
        {
            return;
        }
        time += Time.deltaTime;
        if (time >= 4f)
        {
            audioS.clip = AmbientSound[Random.Range(0, AmbientSound.Length)];
            audioS.Play();
            time = 0;
        }
    }

    void canMovefct()
    {
        canMove = true;
    }
}
