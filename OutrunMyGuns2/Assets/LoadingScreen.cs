using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public int SceneIndex;
    [SerializeField] GameObject textLoading, textEspace;
    [SerializeField] Image slidebar;

    void Awake()
    {
        textLoading.SetActive(true);
        textEspace.SetActive(false);

        SceneIndex = PlayerPrefs.GetInt("SceneIndex");
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneIndex);

        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        slidebar.fillAmount = asyncOperation.progress;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            //Debug.Log("Pro :" + asyncOperation.progress);

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                textLoading.SetActive(false);
                textEspace.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Wait to you press the space key to activate the Scene
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
