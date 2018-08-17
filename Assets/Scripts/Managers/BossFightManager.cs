using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BossFightManager : NetworkBehaviour {

    public PositionSquare leftSquare;
    public PositionSquare centerSquare;
    public PositionSquare rightSquare;

    public Button leftButton;
    public Button rightButton;

    public PlayerPawn pPawn;

    public EnemySpawnedEvent bossSpawnedEvent;

    public BarbarianChieftan barbarianChieftanPrefab;
    public ElderMagician elderWizardPrefab;

    public void Awake()
    {
        bossSpawnedEvent = new EnemySpawnedEvent();
    }

    public PositionSquare getPositionSquare(PositionSquareLocation loc)
    {
        switch (loc)
        {
            case PositionSquareLocation.center:
                return centerSquare;
            case PositionSquareLocation.left:
                return leftSquare;
            case PositionSquareLocation.right:
                return rightSquare;
        }

        return null;
    }

    public PositionSquare getRandomPositionSquare()
    {
        return getPositionSquare((PositionSquareLocation)Random.Range(0, 3));
    }
   
    public void ServerStartFight()
    {
        foreach (PlayerPawn ppawn in FindObjectsOfType<PlayerPawn>())
        {
            Debug.LogError("Caling it");
            ppawn.RpcInitializeStats();
        }
        RpcStartFight();
        SpawnBoss();

    }

    public void SpawnBoss()
    {
        var boss = Instantiate(elderWizardPrefab);
        NetworkServer.Spawn(boss.gameObject);
        RpcNotifyBossSpawn(boss.gameObject);

    }

    [ClientRpc]
    public void RpcNotifyBossSpawn(GameObject boss)
    {
        bossSpawnedEvent.Invoke(boss);
            
    }

    [ClientRpc]
	public void RpcStartFight()
    {
        Debug.LogError("Starting fight1: ");
        //FindObjectOfType<PlayerPawn>().MoveToSquare(centerSquare);

        pPawn = FindObjectOfType<Player>().GetPlayerPawn();

        pPawn.CmdMoveSquares(PositionSquareLocation.center);




        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();

        leftButton.onClick.AddListener(pPawn.CmdMoveLeft);
        rightButton.onClick.AddListener(pPawn.CmdMoveRight);


        FindObjectOfType<CombatHud>().Init(0);
    }
}
