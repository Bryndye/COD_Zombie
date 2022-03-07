using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsDirection : MonoBehaviour
{

    public float speed = 5.0f;
    Vector3 direction;
    public void Start()
    {
        direction = new Vector3(Random.value, Random.value, 0);
        Debug.Log(direction);
    }


    public void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
