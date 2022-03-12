using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementInteractable : MonoBehaviour
{
    public bool isShootable = false;
    public bool isUsed = false;
    public UnityEvent AfterInteractable;

    public void ActivateElement()
    {
        if (isUsed)
        {
            return;
        }
        isUsed = true;
        AfterInteractable.Invoke();
    }
}
