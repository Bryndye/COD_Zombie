using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public int Cost;

    public UnityEvent OpenTheDoor;

    public void RemoveTheDoor()
    {
        gameObject.SetActive(false);
    }
}
