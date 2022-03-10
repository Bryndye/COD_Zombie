using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    [Header("Litteraly points")]
    public int Points;
    [SerializeField] int startingPoints = 500;

    [Header("Param")]
    public Animator AnimPoint;
    [SerializeField] GameObject prefabPointText;
    [SerializeField] Transform parentPoints;
    int index = 0;

    [Header("UI")]
    [SerializeField] int numberOfPointText = 30;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI tCost;
    public List<TextMeshProUGUI> PointsT;


    private void Start()
    {
        Points = startingPoints;
        for (int i = 0; i < numberOfPointText; i++)
        {
            PointsT.Add(Instantiate(prefabPointText, parentPoints).GetComponent<TextMeshProUGUI>());
        }
    }

    private void Update()
    {
        pointsText.text = Points.ToString();
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetPoints(10);
        }
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
        PointsT[index].GetComponent<PointsDirection>().ResetState();
        PointsT[index].text = "+" + _points.ToString();

        index++;
        if (index >= PointsT.Count)
        {
            index = 0;
        }
    }
}