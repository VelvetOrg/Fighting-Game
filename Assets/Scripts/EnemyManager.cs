/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/
using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
  
    public static int enemyCount;

    void Start()
    {
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        for (int i = 0; i < allEnemies.Length; i++)
            enemyCount ++;
		Enemy.OnEnemyDeath += OnEnemyKilled;
        Debug.Log(enemyCount + " enemies in scene.");
    }
      	
	void OnEnemyKilled()
	{
		enemyCount--;
	}
}