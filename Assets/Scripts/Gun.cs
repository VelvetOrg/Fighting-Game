/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//Needs to inherit from meele weapon
public class Gun : RangedWeapon
{
    //There are three gun modes
    [System.Serializable]
    public enum Mode { Automatic, Burst, Manual };
    public Mode fireMode = Mode.Manual;
    public float fireRate = 100.0f;

    //Check for manual fire
    bool triggerReleased;

    //Actually fire the gun
    protected override void Fire()
    {
        print("Shot fired.");
    }

    //When buttons are pressed
    public override void AttackHeld() { Fire(); triggerReleased = false; }
    public override void AttackReleased() { triggerReleased = true; }
}
