using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem Instance;
    public GameObject PrefabVisual;
    public AudioSource PrefabAudio;

    public int CountSInit = 20, CountVInit = 20;

    public List<AudioSource> SFXs;
    public List<GameObject> VFXs;
    int currentSIndex = 0, currentVIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < CountSInit; i++)
        {
            SFXs.Add(Instantiate(PrefabAudio, transform));
        }
        for (int y = 0; y < CountVInit; y++)
        {
            VFXs.Add(Instantiate(PrefabVisual, transform));
        }
    }

    public void SetVFx()
    {

    }

    #region Audio Fx Manager
    public void SetSfx(AudioClip _clip, Vector3 _pos)
    {
        SFXs[currentSIndex].transform.position = _pos;
        PlaySfx(_clip);
    }

    public void SetSfx(AudioClip _clip, Transform _parent)
    {
        SFXs[currentSIndex].transform.parent = _parent;
        SFXs[currentSIndex].transform.localPosition = Vector3.zero;
        PlaySfx(_clip);
    }

    private void PlaySfx(AudioClip _clip)
    {
        SFXs[currentSIndex].clip = _clip;
        SFXs[currentSIndex].Play();
        //Invoke(nameof(ShutDownSFX(SFXs[currentIndex])), SFXs[currentIndex].clip.length);

        currentSIndex++;
        if (currentSIndex >= SFXs.Count)
        {
            currentSIndex = 0;
        }
    }
    private void ShutDownSFX(AudioSource _sfx)
    {
        _sfx.Stop();
    }
    #endregion


}
