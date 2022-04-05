using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Generator : ItemInteraction
{
    public bool IsActivated = false;

    public UnityEvent GeneratorOn;

    public override void Interact(PlayerActions _pA)
    {
        //base.Interact();
        IsActivated = true;
        GeneratorOn.Invoke();
    }
    public void ActivatedGenerator()
    {
        IsActivated = true;
        GeneratorOn.Invoke();
    }
}
