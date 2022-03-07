using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsDirection : MonoBehaviour
{

    public float speed = 5.0f;
    Vector3 direction;
    public float TimeMax = 2f;
    float time = 0;

    public void Start()
    {
        float _rngY = Random.Range(-7, 8);
        direction = new Vector3(10, _rngY, 0);
    }


    public void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        time += Time.deltaTime;
        if (time >= TimeMax)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetState()
    {
        transform.localPosition = new Vector3(50,0,0);
        time = 0;
    }
}
