using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EnemySpawnedEvent : UnityEvent<GameObject>{

}

public class AreaLoadedEvent : UnityEvent<AreaInfoStruct>
{

}

[System.Serializable]
public struct AreaInfoStruct
{
    public string areaName;
    public int index;
    public int minLevel;
    public int maxLevel;
    public Enemy[] enemies;

}


public class FightManager : NetworkBehaviour {

    EnemySpawner enemySpawner;
    //Player player;

    public EnemySpawnedEvent enemySpawnedEvent;
    public AreaLoadedEvent areaLoadedEvent;

    public AreaInfoStruct[] areaInformation;
    public AreaInfoStruct currentArea;

    public int enemiesAlive = 0;
    
    public int area = 0;

    void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
       // player= FindObjectOfType<Player>();

        enemySpawnedEvent = new EnemySpawnedEvent();
        areaLoadedEvent = new AreaLoadedEvent();
    }


    public void startCombat()
    {
        if (!isServer)
        {
            return;
        }
        //player.GetPlayerPawn().InitializeStats(player.GetComponent<PlayerStats>().GetTotalStatStruct());

        //Tell clients to update playerpawn stats
       // RpcPrepareCombat();
       
        foreach(PlayerPawn ppawn in FindObjectsOfType<PlayerPawn>())
        {
            Debug.LogError("Caling it");
            ppawn.RpcInitializeStats();
            ppawn.transform.position = new Vector3(0, -3, 0);
        }

        RpcClientStartCombat(area);
        
        if(area == 6)
        {
            //Endless tower
            GetComponent<EndlessTower>().StartTower(); 
        }else
        {
            //Regular spawning
            SpawnNextWave();
        }
    }

    [ClientRpc]
    void RpcClientStartCombat(int areaNumber)
    {
        FindObjectOfType<CombatHud>().Init(areaNumber);
        currentArea = areaInformation[areaNumber];
        areaLoadedEvent.Invoke(currentArea);
    }

    [ClientRpc]
    public void RpcNotifyEnemySpawn(GameObject enemy)
    {
        enemySpawnedEvent.Invoke(enemy);
        //player.GetPlayerPawn().InitializeStats(player.GetComponent<PlayerStats>().GetTotalStatStruct());

    }

    public void SetArea(int areaNumber)
    {
        area = areaNumber;
        currentArea = areaInformation[area];
    }



    public void SpawnNextWave()
    {
        int numEnemies = Random.Range(1, 4);
        List<Enemy> spawnedEnemies = new List<Enemy>();

        enemiesAlive += numEnemies;

        for (int i = 0; i < numEnemies; i++)
        {
            var enemy = enemySpawner.SpawnEnemy(currentArea);
            enemy.unitDied.AddListener(enemyHasDied);
            RpcNotifyEnemySpawn(enemy.gameObject);
            spawnedEnemies.Add(enemy);
        }

        if(numEnemies == 1)
        {
            spawnedEnemies[0].transform.position = new Vector3(0, 1, 0);
        }

        //Positioning enemies
        if(numEnemies == 2)
        {
            spawnedEnemies[0].transform.position = spawnedEnemies[0].transform.position + new Vector3(-1.5f, 1, 0);
            spawnedEnemies[1].transform.position = spawnedEnemies[1].transform.position + new Vector3(1.5f, 1, 0);
        }

        if (numEnemies == 3)
        {
            spawnedEnemies[1].transform.position = spawnedEnemies[1].transform.position + new Vector3(-1.5f, 2, 0);
            spawnedEnemies[2].transform.position = spawnedEnemies[2].transform.position + new Vector3(1.5f, 2, 0);
        }

    }
    

    public void RemoveWave()
    {
        var enemies = FindObjectsOfType<Enemy>();
        for(int i = enemies.Length; i > 0; i--)
        {
            Destroy(enemies[i-1].gameObject);
        }
        enemiesAlive = 0;
    }

    [Command]
    public void CmdRunFromWave()
    {
        RemoveWave();
        SpawnNextWave();
    }

    public void enemyHasDied(Unit unit)
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            SpawnNextWave();
        }
    }


    public void BackToMainMenu()
    {
        NetworkManager.singleton.ServerChangeScene("HomeScene");

    }

}
