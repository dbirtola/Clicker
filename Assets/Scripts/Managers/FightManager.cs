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

    const float chanceToSpawnFromPreviousArea = 0.4f;

    EnemySpawner enemySpawner;
    //Player player;

    public Shrine shrinePrefab;

    public EnemySpawnedEvent enemySpawnedEvent;
    public EnemySpawnedEvent shrineSpawnedEvent;

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
        shrineSpawnedEvent = new EnemySpawnedEvent();
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

    [ClientRpc]
    public void RpcNotifyShrineSpawn(GameObject shrine)
    {
        shrineSpawnedEvent.Invoke(shrine);
    }

    public void SetArea(int areaNumber)
    {
        area = areaNumber;
        currentArea = areaInformation[area];
    }



    public void SpawnNextWave()
    {
        int numEnemies = 1;
        //30% chance for 2 enemies
        if(Random.Range(0, 3) == 0)
        {
            numEnemies++;
            //10% chance for 3 enemies
            if(Random.Range(0, 3) == 0)
            {
                numEnemies++;
            }
        }

        List<Enemy> spawnedEnemies = new List<Enemy>();

        enemiesAlive += numEnemies;

        for (int i = 0; i < numEnemies; i++)
        {
            Enemy enemy = null;
            int level = Random.Range(currentArea.minLevel, currentArea.maxLevel + 1);

            //Determine enemy
            if(Random.Range(0, 1f) < chanceToSpawnFromPreviousArea){
                //Spawn from a previous zone
                AreaInfoStruct area = areaInformation[Random.Range(0, currentArea.index + 1)];
                enemy = enemySpawner.SpawnEnemy(area, level);
            }else
            {
                //Spawn from this zone
                enemy = enemySpawner.SpawnEnemy(currentArea);
            }

            if(numEnemies == 2)
            {
                //15% bonus exp if theres 2 enemies in the wave
                enemy.AddExperienceMultiplier(1.15f);
            }

            if(numEnemies == 3)
            {
                //30% bonus exp if theres 3 enemies in the wave
                enemy.AddExperienceMultiplier(1.3f);
            }

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
            enemies[i - 1].CleanUp();
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
            //Determine if wave or shrine

            if(Random.Range(0, 2) == 0)
            {
                SpawnShrine();
            }else
            {
                SpawnNextWave();
            }
        }
    }


    public void SpawnShrine()
    {
        var shrine = Instantiate(shrinePrefab, new Vector3(0, 1, 0), Quaternion.identity);
        NetworkServer.Spawn(shrine.gameObject);

        shrine.StartShrineEvent();
        shrineSpawnedEvent.Invoke(shrine.gameObject);

        shrine.shrineEventFinishedEvent.AddListener(() => { ShrineFinished(shrine); });
    }

    public void ShrineFinished(Shrine shrine)
    {
        //SpawnNextWave();
        StartCoroutine(ShrineFinishedRoutine(shrine));
    }

    IEnumerator ShrineFinishedRoutine(Shrine shrine)
    {
        //Fake pause while there is no shrine death animation
        yield return new WaitForSeconds(1.5f);
        Destroy(shrine.gameObject);
        SpawnNextWave();
    }

    public void BackToMainMenu()
    {
        PersistentHud.persistentHud.CoverSceneTransition();

        NetworkManager.singleton.ServerChangeScene("HomeScene");

    }

}
