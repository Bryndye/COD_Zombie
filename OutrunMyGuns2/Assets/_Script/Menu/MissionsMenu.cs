using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionsMenu : MonoBehaviour
{
    [Header("Start Event")]
    [SerializeField] UnityEvent PressStart;
    bool started = false;

    [Header("Missions Setup")]
    //[SerializeField] Transform ListBtnT;
    //[SerializeField] GameObject BtnPrefab;
    [SerializeField] Image IllustrationPlace;
    [SerializeField] TextMeshProUGUI DecriptionPlace;
    [SerializeField] Mission[] Missions;



    private void Start()
    {
        foreach (Mission item in Missions)
        {
            //GameObject _btn = Instantiate(BtnPrefab, ListBtnT);
            item.BtnInstance.name = "Btn_" + item.NameMission;
            item.BtnInstance.GetComponentInChildren<TextMeshProUGUI>().text = item.NameMission;
            item.BtnInstance.onClick.AddListener(delegate { LaunchLevel(item.IndexSceneBuild); });
            //_btn.GetComponent<EventTrigger>().OnPointerEnter(OnPointerEnterBtnMission());
        }
        OnPointerEnterBtnMission(0);
    }


    private void Update()
    {
        //Add input any key
        if (!started)
        {
            PressStart.Invoke();
            started = true;
        }
    }
    public void OnPointerEnterBtnMission(int _indexList)
    {
        IllustrationPlace.sprite = Missions[_indexList].Illustration;
        DecriptionPlace.text = Missions[_indexList].NameMission;
    }

    public void LaunchLevel(int _indexLevel) 
    {
        PlayerPrefs.SetInt("SceneIndex", _indexLevel);
        SceneManager.LoadScene("LoadingScreen");
    }
}

[Serializable]
public class Mission
{
    public Button BtnInstance;
    public string NameMission = "Mission", Description = "Text example description";
    public int IndexSceneBuild = 0;
    public Sprite Illustration;
}