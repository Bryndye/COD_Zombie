using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem Instance;
    public int CountFxInstance = 20;
    public List<AudioSource> SFXs;
    int currentIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSfx(AudioClip _clip, Vector3 _pos)
    {
        SFXs[currentIndex].transform.position = _pos;
        PlaySfx();
    }

    public void SetSfx(AudioClip _clip, Transform _parent)
    {
        SFXs[currentIndex].transform.parent = _parent;
        PlaySfx();
    }

    private void PlaySfx()
    {
        SFXs[currentIndex].Play();
        //Invoke(nameof(ShutDownSFX(SFXs[currentIndex])), SFXs[currentIndex].clip.length);

        currentIndex++;
        if (currentIndex >= SFXs.Count)
        {
            currentIndex = 0;
        }
    }

    private void ShutDownSFX(AudioSource _sfx)
    {
        _sfx.Stop();
    }
}
