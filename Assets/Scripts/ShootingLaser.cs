using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLaser : MonoBehaviour
{
    public void ShootLaser() {
        PickupManager PM = FindObjectOfType<PickupManager>();
        if (PM) {
            PM.FireMagicShot();
        } else if (!PM) print("ShootnigLaser/ShootLaser: pickupManager not found.");
    }
}
