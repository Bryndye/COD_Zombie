using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem Instance;
    public int CountFxInstance = 20;
    public List<ParticleSystem> FX;
    int currentIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void SteFx(Vector3 _pos)
    {

    }
}
