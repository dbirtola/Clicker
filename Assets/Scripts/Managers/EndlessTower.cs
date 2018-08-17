using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EndlessTower : MonoBehaviour {

    FightManager fm;
    EnemySpawner enemySpawner;

    public UnityEvent towerFinishedEvent;
   
    public TowerHUD towerHUD;
    public GameObject runButton; //Needs to be disabled when tower starts, no running is allowed in the tower

    public int towerStartLevel = 0;
    public int currentLevel = 0;
    public int artifactPointsEarned;

    public int[] artifactPointsRewarded;

    float towerDuration = 60;

    float finishTime;

    public void Awake()
    {
        towerFinishedEvent = new UnityEvent();

        fm = FindObjectOfType<FightManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    //Needs to be reworked for multipayer. This should be a client rpc with the tower level decided by the server
    public void StartTower()
    {
        towerHUD.gameObject.SetActive(true);
        runButton.gameObject.SetActive(false); //no running in the tower

        towerStartLevel += FindObjectOfType<PlayerStats>().GetTowerStartBonus();
        currentLevel = towerStartLevel;
        SpawnTowerMonster(towerStartLevel);
        StartCoroutine(TowerRoutine());
    }

    IEnumerator TowerRoutine()
    {

        finishTime = Time.time + towerDuration;

        yield return new WaitForSeconds(towerDuration);

        ProcessResults();
        Destroy(FindObjectOfType<Enemy>());


    }

    public void ProcessResults()
    {
        towerFinishedEvent.Invoke();
    }
    
    public float GetTimeRemaining()
    {
        return finishTime - Time.time;
    }

    public void SpawnTowerMonster(int level)
    {
        //Make sure to leave as integer division
        int areaToPickFrom = level / 10;
        Debug.Log("Picking a monster from: " + fm.areaInformation[areaToPickFrom].areaName);
        var enemy = enemySpawner.SpawnEnemy(fm.areaInformation[areaToPickFrom], level);
        enemy.unitDied.AddListener(towerMonsterDied);
        fm.RpcNotifyEnemySpawn(enemy.gameObject);
    }

    public void towerMonsterDied(Unit monster)
    {

        int artifactPoints = artifactPointsRewarded[currentLevel / 10];
        artifactPointsEarned += artifactPoints;

        currentLevel++;
        SpawnTowerMonster(currentLevel);
    }
}
