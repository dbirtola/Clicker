using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class EnemySpawner : NetworkBehaviour {

    PlayerStats playerStats;

    public GameObject slimePrefab;
    public GameObject killerBeePrefab;
    public GameObject barbarianPrefab;

    public Enemy enemyPrefab;

   
    public float chanceForSpecialMonster = 0.1f;

	public override void OnStartServer()
    {
        //The special monster multiplier is based on the hosts stats
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public Enemy SpawnEnemy(AreaInfoStruct areaInfo, int level = -1)
    {

        Enemy enemy = null;

        int roll = Random.Range(0, areaInfo.enemies.Length);

        enemy = Instantiate(areaInfo.enemies[roll], new Vector3(0, 0, 0), Quaternion.identity);

        if(level == -1)
        {
            enemy.SetLevel(Random.Range(areaInfo.minLevel, areaInfo.maxLevel + 1));
        }else
        {
            enemy.SetLevel(level);
        }

        //Chance to spawn magic enemy improved by hosts special monster multiplier
        if(Random.Range(0, 1f) <= chanceForSpecialMonster * playerStats.GetSpecialMonsterMultiplier())
        {
            var me = enemy.gameObject.AddComponent<MagicEnemy>();
            Debug.Log("Spawned Magic enemy!");
        }

        NetworkServer.Spawn(enemy.gameObject);

        return enemy.GetComponent<Enemy>();
    }

    public void AddSpecialChance(float chance)
    {
        chanceForSpecialMonster += chance;
    }
}
