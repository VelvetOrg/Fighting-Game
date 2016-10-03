/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//Needs to inherit from meele weapon
[RequireComponent(typeof(LineRenderer))]
public class Gun : RangedWeapon
{
    //There are three gun modes
    [System.Serializable]
    public enum Mode { Automatic, Burst, Single };
    public Mode fireMode = Mode.Single;

    public int burstCount = 3;
    public float fireRate = 100.0f;

    //An optional laymask apply to the raycast
    public LayerMask ignoreLayers;

    //Check for manual fire
    float nextShoot;
    bool triggerReleased;
    int burstShotsRemaining;

    //For raycasting
    Camera main;
    LineRenderer line;
    WaitForSeconds shotDuration;

    //Set
    void Start()
    {
        //Privates need default values
        triggerReleased = true;
        burstShotsRemaining = burstCount;

        //Get raycasting things
        main = Camera.main;
        line = GetComponent<LineRenderer>();
        shotDuration = new WaitForSeconds(attackDuration);
    }

    //Temporary - the line renderer is always at the current position
    void Update() { line.SetPosition(0, end.position); }

    //Actually fire the gun
    protected override void Fire()
    {
        //Timer
        if (Time.time > nextShoot)
        {
            //Lower burst shot count
            if (fireMode == Mode.Burst)
            {
                if (burstShotsRemaining <= 0) return;
                burstShotsRemaining--;
            }

            //Just fire once when held
            if (fireMode == Mode.Single) { if (!triggerReleased) { return; } }

            //Wait for auto fire
            nextShoot = Time.time + (fireRate / 1000.0f);

            //Demo
            StartCoroutine(shootEffect());

            //Fire out a ray from the exact center of the camera
            //Then get information about the raycast
            Vector3 rayOrigin = main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            //Draw a line from gun to the resultting raycast
            if (Physics.Raycast(rayOrigin, main.transform.forward, out hit, range, ignoreLayers))
            {
                //Draw
                line.SetPosition(1, hit.point);

                //Apply a physics force to the obect if it has a rigidbody
                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    //Apply a force
                    hit.rigidbody.AddForce(-hit.normal * 100.0f);
                }
            }

            else { line.SetPosition(1, main.transform.position + (main.transform.forward * 50)); }
        }
    }

    //When buttons are pressed
    public override void AttackHeld() { Fire(); triggerReleased = false; }
    public override void AttackReleased() { triggerReleased = true; burstShotsRemaining = burstCount; }

    //The temporary effect to play when shooting
    //From: https://unity3d.com/learn/tutorials/lets-try/shooting-with-raycasts
    IEnumerator shootEffect()
    {
        //Draw line renderer then wait
        line.enabled = true;
        yield return shotDuration;
        line.enabled = false;
    }
}
