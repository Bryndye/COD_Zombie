using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager InstanceMM;

    [SerializeField] Menu[] menus;

    [SerializeField] string BackToTitleName;

    private void Awake()
    {
        InstanceMM = this;
        for (int i = 0; i < menus.Length; i++)
        {
            if (!menus[i].Openned)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    private void Start()
    {
        //OpenMenu(BackToTitleName);
    }

    #region Menu Objects Open/Close
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].Name == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].Openned)
            {
                CloseMenu(menus[i]);
            }
        }

        //SM.GetASound("ButtonUIClick", transform, true);
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].Openned)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
        //SM.GetASound("ButtonUIClick", transform, true);
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
    #endregion

    #region Fct basic Menu

    public void OpenScene(int _index)
    {
        SceneManager.LoadScene(_index);
    }

    public void QuitApplication()
    {
        //SM.GetASound("ButtonUIClick", transform, true);
        Application.Quit();
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //OpenMenu(BackToTitleName);
        }
    }
}
