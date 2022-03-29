using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    BonusManager bonusManager;

    [Header("Litteraly points")]
    public int Points;
    [SerializeField] int startingPoints = 500;

    [Header("Param")]
    public GameObject AnimPoint;
    [SerializeField] GameObject prefabPointText;
    [SerializeField] Transform parentPoints;
    int index = 0;

    [Header("Stats Player")]
    public int FinalPoints, Kills, HeadShoots, Deads;

    [Header("UI")]
    [SerializeField] int numberOfPointText = 30;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI tCost;
    public List<TextMeshProUGUI> PointsT;

    private void Awake()
    {
        FinalPoints = startingPoints;
    }
    private void Start()
    {
        bonusManager = BonusManager.Instance;
        Points = startingPoints;
        for (int i = 0; i < numberOfPointText; i++)
        {
            PointsT.Add(Instantiate(prefabPointText, parentPoints).GetComponent<TextMeshProUGUI>());
        }
    }

    private void Update()
    {
        pointsText.text = Points.ToString();
    }


    public bool CanPlayerBuyIt(int _cost)
    {
        if (_cost > Points)
        {
            //feedback cant ?
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Buy(int _cost)
    {
        if (!CanPlayerBuyIt(_cost))
        {
            return;
        }
        Points -= _cost;
        tCost.text = "-" + _cost.ToString();
        AnimPoint.SetActive(false);
        AnimPoint.SetActive(true);
        //Feedback buy sound !!!!!!!!!!
    }

    public void GetPoints(int _points)
    {
        int _finalPoints;
        if (bonusManager != null)
        {
            if (bonusManager.IsDoublePoints)
            {
                _finalPoints = _points * 2;
            }
            else
            {
                _finalPoints = _points;
            }
        }
        else
        {
            _finalPoints = _points;
        }
        Points += _finalPoints;
        FinalPoints += _finalPoints;

        PointsT[index].gameObject.SetActive(true);
        PointsT[index].GetComponent<PointsDirection>().ResetState();
        PointsT[index].text = "+" + _finalPoints.ToString();

        index++;
        if (index >= PointsT.Count)
        {
            index = 0;
        }
    }

    private void TimeLapse()
    {

    }

    public void GetStats(bool _headShoot)
    {
        Kills++;
        if (_headShoot)
        {
            HeadShoots ++;
        }
    }
}
