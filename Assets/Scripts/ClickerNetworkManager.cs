using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClickerNetworkManager : NetworkManager {
    
    int destinationArea = 0;

	// Use this for initialization
	void Start () {
       
        //StartHost();
	}
	


    public void SetDestinationArea(int area)
    {
        destinationArea = area;
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if(sceneName == "FightScene")
        {
            //FindObjectOfType<FightManager>().startCombat();
            StartCoroutine(theThing());
        }
        if (sceneName == "HomeScene")
        {
            FindObjectOfType<InventoryUI>().Refresh();
        }
        if(sceneName == "BossScene")
        {
            StartCoroutine(theBossThing());

        }
    }

    IEnumerator theThing()
    {
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<FightManager>().SetArea(destinationArea);
        FindObjectOfType<FightManager>().startCombat();

    }

    IEnumerator theBossThing()
    {
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<BossFightManager>().ServerStartFight();

    }

    public void ConnectAsClient()
    {
        StopHost();
        StartClient();
    }
}
