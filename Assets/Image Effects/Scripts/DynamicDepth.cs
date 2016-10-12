/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//THis changes the focus of the dewpth of field
public class DynamicDepth : MonoBehaviour
{
    //Holds the transform that is the camera focus
    public Transform depth;

    //Private
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        //Fire a ray
        ray = new Ray(transform.position, transform.forward);
        hit = new RaycastHit();

        bool change = Physics.Raycast(ray, out hit);

        //Update the transfroms position
        if(change) { depth.transform.position = hit.point; }
    }
}
