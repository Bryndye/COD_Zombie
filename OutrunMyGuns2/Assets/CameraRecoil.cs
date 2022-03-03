using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] PlayerWeapon pw;
    [Header("Recoil")]
    Vector3 currentRot, targetRot;



    void Update()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, pw.currentWeapon.returnSpeed * Time.deltaTime);
        currentRot = Vector3.Slerp(currentRot, targetRot, pw.currentWeapon.snapiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRot);
    }

    public void RecoilFire()
    {
        targetRot += new Vector3(pw.currentWeapon.recoil.x, Random.Range(-pw.currentWeapon.recoil.y, pw.currentWeapon.recoil.y),
            Random.Range(-pw.currentWeapon.recoil.z, pw.currentWeapon.recoil.z));
    }
}
