using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Allinteractions : MonoBehaviour
{
    public List<ElementInteractable> Elements;

    public UnityEvent AllInteractionDone;
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
        AllInteractionDone.Invoke();
    }
}
