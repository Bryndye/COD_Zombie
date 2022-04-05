using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EasterEggsManager : MonoBehaviour
{
    public List<ItemEasterEgg> Elements;

    public UnityEvent EasterEggsDone;
    bool done = false;

    private void Update()
    {
        if (done)
        {
            return;
        }
        foreach (var item in Elements)
        {
            if (!item.isUsed)
            {
                return;
            }
        }
        done = true;
        EasterEggsDone.Invoke();
    }
}
