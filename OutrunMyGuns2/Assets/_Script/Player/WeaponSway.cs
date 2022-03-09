using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float Amount, MaxAmount, SmoothAmount;

    Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    private void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * Amount;
        float movementY = -Input.GetAxis("Mouse Y") * Amount;

        movementX = Mathf.Clamp(movementX, -MaxAmount, MaxAmount);
        movementY = Mathf.Clamp(movementY, -MaxAmount, MaxAmount);

        Vector3 finalPos = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, SmoothAmount * Time.deltaTime);
    }
}
