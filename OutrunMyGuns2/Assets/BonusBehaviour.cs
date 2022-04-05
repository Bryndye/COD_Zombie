using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBehaviour : MonoBehaviour
{
    public BonusType MyBonus;
    public float TimeMax = 30, TimeRunning = 0;
    [SerializeField] GameObject visual;
    public float timeAnimMax = 14f, timeAnim = 0;
    public float timerotMax = 2f, timeRotAnim = 0;

    public Vector3 targetRot;
    void Update()
    {
        TimeRunning += Time.deltaTime;
        timeRotAnim += Time.deltaTime;

        if (Mathf.Round(TimeRunning) % 2 == 0 && timeRotAnim >= timerotMax)
        {
            timeRotAnim = 0;
            targetRot = new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90));
        }
        visual.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(visual.transform.localEulerAngles.x, targetRot.x, 0.1f),
            Mathf.LerpAngle(visual.transform.localEulerAngles.y, targetRot.y, 0.1f),
            Mathf.LerpAngle(visual.transform.localEulerAngles.z, targetRot.z, 0.1f));

        timeAnim += Time.deltaTime;
        if (timeAnim >= timeAnimMax)
        {
            timeAnim = 0;
            timeAnimMax /= 2;
            visual.SetActive(false);
            Invoke(nameof(activate), timeAnimMax / 4);
        }
        if (TimeRunning >= TimeMax)
        {
            Destroy(gameObject);
        }
    }

    void activate() => visual.SetActive(true);

    private void OnTriggerEnter(Collider other)
    {
        BonusManager.Instance.GetBonus(MyBonus);
        Destroy(gameObject);
    }
}
