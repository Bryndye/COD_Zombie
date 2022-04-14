using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float TimeToExplode = 5f;
    float time = 0;

    public float RadiusExplosion = 5f;

    void Update()
    {
        time += Time.deltaTime;
        if (time >= TimeToExplode)
        {
            ExplodeTheGrenade();
        }
    }

    public void ExplodeTheGrenade()
    {
        //explosion
        Destroy(gameObject);
    }
           
}
