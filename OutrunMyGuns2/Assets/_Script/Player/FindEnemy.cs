using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemy : MonoBehaviour
{
    public Transform Enemy; //le transform de l'ennemi que vous voulez target
    public Transform pointer; //Pour debug InGame la rotation/ l'objet que vous voulez rotate
    public float SpeedAngular = 1f;
    private float angle;
    private float alpha;
    private int coefficient; // le coeff sert pour savoir si l'ennemi est à droite ou gauche

    private bool running;

    [Header("init var")]
    Vector3 a, b,c;

    float A, B, C;

    private float timer;
   
    private void Awake()
    {
        pointer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (running && Enemy != null)
        {
            FindHim();

            timer += Time.deltaTime;
            if (timer >= 3)
            {
                EndFinder();
            }
        }
    }

    private void FindHim()
    {
        a = transform.position;                     //Player
        b = transform.position + transform.forward; //Le point devant le Player
        c = Enemy.position;                         //Enemi

        A = Vector3.Distance(b, c);                    //Distance entre b et c
        B = Vector3.Distance(a, c);                   //Distance entre a et c
        C = Vector3.Distance(a, b);                   //Distance entre a et b

        if (B != 0 && C != 0)
        {
            alpha = Mathf.Acos((B * B + C * C - A * A) / (2 * B * C)); // Valeur pi s'il y a 180° de différence entre Player et Enemy

            Vector3 PointTarget = (c - a).normalized;

            if (Vector3.Dot(PointTarget, transform.right) > 0)
            {
                coefficient = -1;
                //Debug.Log("Target is in right");
            }
            else
            {
                coefficient = 1;
                //Debug.Log("Target is left");
            }

            angle = ((alpha * 180) / Mathf.PI) * coefficient;
            if (!float.IsNaN(angle))
            {
                //print("alpha : " + angle + "angle: " + angle);
                pointer.localEulerAngles = new Vector3(pointer.localRotation.x, pointer.localRotation.y, Mathf.LerpAngle(pointer.localEulerAngles.z, angle, SpeedAngular));
            }
        }
    }

    public void StartFinder(Transform en)
    {
        timer = 0;
        Enemy = en;
        running = true;
        pointer.gameObject.SetActive(true);
    }

    private void EndFinder()
    {
        timer = 0;
        running = false;
        pointer.gameObject.SetActive(false);
    }
}

/*
 * A = Player
 * B = Forward
 * C = enemy
 * triangle ABC
 * a² = b²+c² -2bc cos(alpha)
 * b² = a²+c² -2ac cos(beta)
 * c² = a²+b² -2ab cos(yama)
 * 
 * alpha = arccos( (b²+c²-a²) / 2bc)
 *  Mathf.Acos(0.5f) <-- méthode Arcos() qui donne en pi
 * */

