using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClickerNetworkManager : NetworkManager {
    
    int destinationArea = 0;

    static ClickerNetworkManager clickerNetworkManager;

    float minimumTransitionTime = 2f;

	// Use this for initialization
	void Awake () {

        if(clickerNetworkManager == null)
        {
            clickerNetworkManager = this;
        }else
        {
            Destroy(gameObject);
            return;   
        }


        StartHost();
        
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
            StartCoroutine(FightTransition());
        }
        if (sceneName == "HomeScene")
        {
            FindObjectOfType<InventoryUI>().Refresh();
            StartCoroutine(HomeTransition());
        }
        if(sceneName == "BossScene")
        {
            StartCoroutine(BossTransition());

        }

    }

    //The delays below are added for two reasons
    //One, it lets the networked code catch up and most likely all clients will be loaded in by the time the delay is over
    //There is most likely a better way to handle this, possibly with Commands signifying that each client has loaded succesfully. Should do that later
    //The other reason is if someone loads too quickly, they won't have time to see the loading tip. So in a sense it is also an artificial load time, short enough to
    //Not bother people but long enough to let you read the tip hopefully
    IEnumerator HomeTransition()
    {

        yield return new WaitForSeconds(minimumTransitionTime);

        PersistentHud.persistentHud.EndSceneTransition();
    }
    IEnumerator FightTransition()
    {
        yield return new WaitForSeconds(minimumTransitionTime);
        FindObjectOfType<FightManager>().SetArea(destinationArea);
        FindObjectOfType<FightManager>().startCombat();

        PersistentHud.persistentHud.EndSceneTransition();
    }

    IEnumerator BossTransition()
    {
        yield return new WaitForSeconds(minimumTransitionTime);
        FindObjectOfType<BossFightManager>().ServerStartFight();

        PersistentHud.persistentHud.EndSceneTransition();
    }

    public void ConnectAsClient()
    {
        StopHost();
        StartClient();
    }
}
