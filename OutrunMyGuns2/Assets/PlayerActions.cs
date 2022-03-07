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
        if (Physics.Raycast(pc.cameraTransform.position, pc.cameraTransform.forward, out hit, distanceMaxInteraction))
        {
            if (hit.collider.TryGetComponent(out PerkBoitier _pkb) && !perksP.AlreadyHas(_pkb.MyPerk))
            {
                interactText.text = "Press E to buy " + _pkb.MyPerk.ToString() + " for " + _pkb.Cost;

                if (Input.GetKeyDown(KeyCode.E) && pPoints.CanPlayerBuyIt(_pkb.Cost))
                {
                    perksP.WhichPerkToBoy(_pkb.MyPerk);
                    pPoints.Buy(_pkb.Cost);
                }
                //_pkb.
            }
            else
            {
                interactText.text = "";
            }
        }
        else
        {
            interactText.text = "";
        }
    }
}
