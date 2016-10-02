/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

//Namespaces
using UnityEngine;

//A pure abstract class used by the player
public abstract class Weapon
{

}

//Every wepon in the game inherits from this class
//They share some properties in common
// - Gun
// - Bow and arrow
public class RangedWeapon : MonoBehaviour
{
    //Expand on this later
    [Header("Ranged Weapon")]
    public Transform end; //The barrel or fire point
    public float damage = 1.0f; //Result of the attack
    public float attackDuration = 0.5f; //How long each shot lasts for
    public float range = 50.0f; //Distance used by the ray

    //Virtual functions
    protected virtual void Fire() { }

    //Called when the user is holding down fire
    public virtual void AttackReleased() { }
    public virtual void AttackHeld() { Fire(); }
}

//Any meele weapon inherits from this
// - Swords
// - Knives
//Will do this later...
public class MeeleWeapon : MonoBehaviour { }