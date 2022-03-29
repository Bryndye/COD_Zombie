using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsDirection : MonoBehaviour
{
    Animator anim;
    float speedMax = 5;
    public float speed = 5.0f;
    Vector3 direction;
    [Range(1, 2)]
    public float TimeMax = 2f;
    float time = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Start()
    {
        float _rngY = Random.Range(-7, 8);
        direction = new Vector3(10, _rngY, 0);
        anim.SetFloat("TimeFactor", Random.Range(1, TimeMax));

        speed = Random.Range(2, speedMax);
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
