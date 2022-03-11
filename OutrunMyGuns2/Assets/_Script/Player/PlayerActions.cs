using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    PlayerController pc;
    PerksPlayer perksP;
    PlayerPoints pPoints;
    [SerializeField] float distanceMaxInteraction;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] TextMeshProUGUI interactText;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
        perksP = GetComponent<PerksPlayer>();
        pPoints = GetComponent<PlayerPoints>();
    }

    void Update()
    {
        CheckFront();
    }
    private void CheckFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(pc.cameraTransform.position, pc.cameraTransform.forward, out hit, distanceMaxInteraction, ~ignoreLayer))
        {
            if (hit.collider.TryGetComponent(out PerkBoitier _pkb) && !perksP.AlreadyHas(_pkb.MyPerk))
            {
                Debug.Log("perks !");
                interactText.text = "Press E to buy " + _pkb.MyPerk.ToString() + " for " + _pkb.Cost;

                if (Input.GetKeyDown(KeyCode.E) && pPoints.CanPlayerBuyIt(_pkb.Cost))
                {
                    perksP.WhichPerkToBoy(_pkb.MyPerk);
                    pPoints.Buy(_pkb.Cost);
                }
                //_pkb.
            }
            else if (hit.collider.TryGetComponent(out Window _w))
            {
                if (_w.Full)
                {
                    interactText.text = "";
                    return;
                }

                interactText.text = "Press E to rebuild";

                if (Input.GetKey(KeyCode.E))
                {
                    _w.Rebuild(pPoints);
                }
            }
            else
            {
                Debug.Log(hit.collider.name);
                interactText.text = "";
            }
        }
        else
        {
            interactText.text = "";
        }
    }
}
