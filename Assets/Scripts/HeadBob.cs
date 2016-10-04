/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
 */

using UnityEngine;
using System.Collections;

//This class will subtly apply a sin wave to the local position of the obejct is attached to
public class HeadBob : MonoBehaviour
{
    public float horizontallAmount = 0.1f; //Amount to bob head by
    public float verticalAmount = 0.1f; //Amount to bob head by
    public float speed = 1.0f; //How fast to bob

    float height; //How high off the ground if is the character
    float counter; //Counters the current position in the sin wave
    Vector3 parent; //Holds last known position of the parent
    Vector3 initialPosition; //When the game was started pre head bob

    //Get the height of the camera
    //Offset so tha bobs around the the center
    void Start() { height = transform.localPosition.y - (verticalAmount / 2);
        initialPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z); }

	//Needs the position of the movement object
    public void Bob(PlayerController player)
    {
        //Only bob when not jumping
        if (!player.isGrounded()) { parent = player.transform.position; return; }

        //Continue
        //This means that the speed of bobbing will be affected by the character move speed
        counter += Vector3.Distance(parent, player.transform.position) * speed;

        //Update position based on a sine wave
        transform.localPosition = initialPosition + new Vector3(
            Mathf.Sin(counter) * horizontallAmount,
            ((Mathf.Cos(counter * 2) * verticalAmount) * -1) + height, 0);

        //Since the detlta (difference in) position needs to be found
        parent = player.transform.position;
    }
}
