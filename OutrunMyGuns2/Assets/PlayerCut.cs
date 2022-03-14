using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCut : MonoBehaviour
{
    PlayerWeapon playerWeapon;

    [SerializeField] LayerMask ignoreLayerCut;
    [SerializeField] float distanceToCut = 2f;
    [SerializeField] float DurationCutAnim = 1f;
    public bool IsCutting = false;
    public Transform Target;
    [SerializeField] float speedAttract = 0.1f;

    private void Awake()
    {
        playerWeapon = GetComponent<PlayerWeapon>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !IsCutting)
        {
            Cut();
        }
    }

    public void Cut()
    {
        playerWeapon.AnimHands.SetTrigger("Cut");
        playerWeapon.AnimCam.SetTrigger("Cut");
        IsCutting = true;
        Invoke(nameof(EndCut), DurationCutAnim);
        RaycastHit _hit;
        if (Physics.Raycast(playerWeapon.CamTransfrom.position, playerWeapon.CamTransfrom.forward, out _hit, distanceToCut, ~ignoreLayerCut))
        {
            //Debug.Log(_hit.collider.name);
            //if (IS_INSTANTIATE_DEBUG_SHOOT)
            //    Instantiate(PREFAB_TEST_SHOOT, _hit.point, Quaternion.identity);
            if (_hit.collider.TryGetComponent(out ZombieBehaviour _zb))
            {
                _zb.TakeDamage(150, playerWeapon, TypeKill.cut);
                Target = _zb.transform;
            }
            else if (_hit.collider.TryGetComponent(out PartOfBody _head))
            {
                _head.TakeDamage(150, playerWeapon, TypeKill.cut);
                Target = _head.transform;
            }
            Debug.DrawRay(playerWeapon.CamTransfrom.position, playerWeapon.CamTransfrom.forward * 10, Color.blue, 1);
        }
        
    }

    public void EndCut()
    {
        IsCutting = false;
    }
}
