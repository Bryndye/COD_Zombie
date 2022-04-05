using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemEasterEgg : ItemInteraction
{
    public bool isShootable = false;
    public bool isUsed = false;
    public UnityEvent AfterInteractable;

    public override void Interact(PlayerActions _pA)
    {
        DoThing();
    }

    public void ShootMe()
    {
        if (!isShootable)
        {
            return;
        }
        DoThing();
    }

    void DoThing()
    {
        if (isUsed)
        {
            return;
        }
        isUsed = true;
        AfterInteractable.Invoke();
    }
}
