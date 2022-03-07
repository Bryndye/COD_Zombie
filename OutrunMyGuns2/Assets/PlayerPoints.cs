using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public int Points;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] int startingPoints = 500;

    [Header("Anim Points")]
    public Animator AnimPoint;
    [SerializeField] TextMeshProUGUI tCost;
    [SerializeField] GameObject prefabText;
    [SerializeField] Transform parentPoints;
    public List<TextMeshProUGUI> PointsT;
    int index = 0;


    private void Start()
    {
        Points = startingPoints;
        for (int i = 0; i < 20; i++)
        {
            PointsT.Add(Instantiate(prefabText, parentPoints).GetComponent<TextMeshProUGUI>());
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
        Points -= _cost;
        tCost.text = "-" + _cost.ToString();
        AnimPoint.SetTrigger("Active");
        //Feedback buy ?
    }

    public void GetPoints(int _points)
    {
        Points += _points;
        PointsT[index].gameObject.SetActive(true);
        PointsT[index].text = "+" + _points.ToString();
    }
}
