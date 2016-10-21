/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//Requires a NavMeshAgent in order to follow the player
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity
{
    //The player to follow
    public static GameObject player;

    //The range the player must be in for it to stop chasing and instead deal damage
    public static float attackRange = .5f;

    //The time it pauses between attacks
    public float attackPause = 1;

    //Damage dealt to the player per hit
    public float damagePerHit = 1;

    //NavMeshAgent's speed
    public float moveSpeed;

    //Attacking speed
    public float attackSpeed;

    //Total number of hits required to kill the player
    public int hitsToKillPlayer;

    //How often to scan for the player
    [Range(0.2f, 0.5f)]
    public float scanRate;

    //Enum for possible actions that the player can be carrying out
    private enum Action { Idle, Chase, Attack };

    //Current action
    private Action currentlyDoing;

    //Reference to attached NavMeshAgent
    private NavMeshAgent agent;

    //Reference to the entity of the player
    Entity playerEntity;

    //Additional BTS time to delay the attack
    float attackDelay;

    //Enemy "radius"
    static float enemyHalfExtent = 0.5f;

    //Player "radius"
    static float playerHalfExtent = 0.5f;

    //Check if the player is NULL
    bool playerAlive;

	//Event for when any enemy dies
	public static event System.Action OnEnemyDeath;

    //Finds player
    void Awake()
    {
        if (FindObjectOfType<Player>() != null)
        {
            player = FindObjectOfType<Player>().gameObject;
            playerEntity = player.GetComponent<Entity>();
            //playerAlive = (player != null && playerEntity != null) ? true : false;
            playerAlive = true;
        }
    }

    //Sets values
    protected override void Start()
    {
        //Set health
        base.Start();

        //Create reference to NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        //Set the movement speed of 'this' enemy   
        agent.speed = moveSpeed;

        //Calculate how much damage to deal per hit
        if (playerAlive)
            damagePerHit = Mathf.Ceil(playerEntity.initialHealth / hitsToKillPlayer);

        if (playerAlive)
        {
            //Update
            currentlyDoing = Action.Chase;

            //Subscribe the player to delegate
            playerEntity.OnDeath += PlayerDeathEvent;

            //Start chasing the player
            StartCoroutine(ChasePlayer());
        }
    }

    //Checks if 'this' enemy is within attacking range of enemy
    void Update()
    {
        if (playerAlive) //Make sure the player isn't dead
            if (Time.time > attackDelay) //Check if enough time has elapsed to be able attack again
                StartCoroutine(ScanForPlayer()); //Scan for the player
    }

    //Called when the player is killed
    void PlayerDeathEvent()
    {
        //Set to false since player is obviously not alive
        playerAlive = false;
        
        //Set the current action to idle
        currentlyDoing = Action.Idle;
    }
    
    //Attacks the player
    IEnumerator AttackPlayer()
    {
        //Change state to attacking
        currentlyDoing = Action.Attack;

        //Disable the NavMeshAgent as we no longer want to 'chase' the player
        agent.enabled = false;

        //Calculate which direction to attack towards i.e. towards the player
        Vector3 attackDirection = (player.transform.position - transform.position).normalized;

        //Calculate where to attack on the player
        Vector3 attackPosition = player.transform.position - attackDirection * (enemyHalfExtent);
        
        //How much of the attack is completed (starts at 0 obviously)
        float percent = 0;
        
        //Boolean for whether the player has been attacked yet
        bool hasAppliedDamage = false;

        //Enter loop to attack while the attack hasn't yet completed
        while (percent <= 1)
        {
            //Only apply damage after half the attack is over
            if (percent >= .5f && !hasAppliedDamage)
            {
                //Set to true, as the enemy has now damaged the player
                hasAppliedDamage = true;

                //Damage the player
                playerEntity.Damage(damagePerHit);
            }

            //Increment how much of the attack we are through
            percent += Time.deltaTime * attackSpeed;

            //Animation interpolation to follow
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;

            //Animate an attack using Vector3.Lerp
            transform.position = Vector3.Lerp(transform.position, attackPosition, interpolation);

            //Return out
            yield return null;
        }

        //Go back to chasing
        currentlyDoing = Action.Chase;

        //Re-activate the NavMeshAgent in order to chase
        agent.enabled = true;
    }

    //Handles chasing the player
    IEnumerator ChasePlayer()
    {
        //Check that the player is not dead
        while (playerAlive)
        {
            //Make sure that we are in fact meant to be chasing the player
            if (currentlyDoing == Action.Chase)
            {
                //Find the direction to chase in
                Vector3 chaseDirection = (player.transform.position - gameObject.transform.position).normalized;
                
                //Find where to chase to
                Vector3 chasePosition = player.transform.position - chaseDirection * (enemyHalfExtent + playerHalfExtent + attackRange / 2);

                //Make sure the enemy has not been killed
                if (!dead)
                    agent.SetDestination(chasePosition); //Set the enemy's destination to just in front of the player
            }

            //Wait before checking again
            yield return new WaitForSeconds(scanRate);
        }
    }

    //Scans for whether the player is close enough to attack
    IEnumerator ScanForPlayer()
    {
        //Adds a small delay between each attack
        attackDelay = Time.time + attackPause;
        
        //Check that the player is alive
        if (playerAlive)
        {
            //Check all the colliders around the enemy
            Collider[] rangedColliders = Physics.OverlapSphere(transform.position, Mathf.Pow(attackRange + 0.5f + playerHalfExtent, 2));
            foreach (Collider b in rangedColliders) //Loop through each collider
                if (b.gameObject.GetComponent<Player>() != null) //Check if that collider belongs to the Player
                    StartCoroutine(AttackPlayer()); //Attack the player
        }

        //Wait before checking again if
        yield return new WaitForSeconds(scanRate);
    }
}