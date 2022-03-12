using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] FindEnemy fe;

    [Header("Points life")]
    public float LifeMax = 150, Life = 150;
    [SerializeField] Image bloodScreen;
    public bool IsDead = false;

    [Header("Regen")]
    bool isHurt = false;
    [SerializeField] float TimeToRegenMax = 4f;
    float timeRegen;

    private void Start()
    {
        Life = LifeMax;
    }

    private void Update()
    {
        UpdateUILife();
        Regen();
    }

    void UpdateUILife()
    {
        float _alpha = 1 - Life / LifeMax;

        if (_alpha > 255) _alpha = 255;
        else if (_alpha < 0) _alpha = 0;

        bloodScreen.color = new Color(1, 1, 1, _alpha);
    }

    private void Regen()
    {
        if (isHurt)
        {
            timeRegen += Time.deltaTime;
            if (timeRegen >= TimeToRegenMax)
            {
                Life += 5;
            }
            if (Life >= LifeMax)
            {
                isHurt = false;
                timeRegen = 0;
                Life = LifeMax;
            }
        }
    }

    public void TakeDamage(int _dmg, Transform _zb)
    {
        if (IsDead)
            return;

        Life -= _dmg;

        isHurt = true;
        timeRegen = 0;

        fe.StartFinder(_zb);

        if (Life <= 0)
        {
            IsDead = true;
            //Debug.Log("MORT ");
        }
    }
}
