/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//This class handles postion of the arm so that it will always point to where the user want to fire
//It also handles equipping of weapons
//Not finished yet
public class GunManager : MonoBehaviour
{
    //Publics
    public Transform hand; //The transform that attaches to the gun

    //Holds the currently attached weapon
    [HideInInspector]
    public Weapon currentlyEquipt { get; private set; }

    //Will give the player a weapon
    public void EquipWeapon(Weapon weapon)
    {
        //Just make sure that the publics have been set
        if (hand != null)
        {
            //There is already a gun, remove it
            //Check that gun isnt already added
            //Remove
            if (hand.childCount > 0) { Destroy(hand.GetChild(0).gameObject); Debug.LogWarning("Removed weapon"); }

            //Add the gun prefab
            GameObject instantiatedWeapon = Instantiate(weapon.gameObject, hand, false) as GameObject;
            instantiatedWeapon.name = "Gun Weapon (Instantiated at Runtime)";

            //Set
            currentlyEquipt = instantiatedWeapon.GetComponent<Weapon>();
        }
        else { Debug.LogWarning("No hand on the character"); }
    }
}