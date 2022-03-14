using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class Intro : MonoBehaviour
{
    [Header("Special Intro")]
    [SerializeField] VideoPlayer introVideo;
    [SerializeField] AudioSource introSound;
    bool isPlaying = false;

    private void Update()
    {
        if (introVideo.isPlaying && !isPlaying)
        {
            isPlaying = true;
            introSound.Play();
        }
        else if (!introVideo.isPlaying && isPlaying)
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            introVideo.Stop();
            introSound.Stop();
            SceneManager.LoadScene(1);
            //OpenMenu(BackToTitleName);
        }
    }
}
