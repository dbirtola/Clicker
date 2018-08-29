using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CombatHud : MonoBehaviour {

    public Text areaText;

    public HealthBar enemyHealthBar;

    public EnemyBars enemyBarsPrefab;
    public Text enemyName;

    public FightManager fightManager;
    public BossFightManager bossFightManager;
    public CombatHud combatHud;
    public HealthBar playerHealthBar;
    public ManaBar playerManaBar;
    public ExperienceBar experienceBar;

    public PlayerQuests playerQuests;
    public PlayerPawn playerPawn;

    public FloatingText floatingTextPrefab;

    //public GameObject enemyBars;

        //Need to be stored so they can be removed when the player leaves the scene, or else they stack up every time
        //The player joins the area
    UnityAction<GameObject, int> showDamageDealtFunction;
    UnityAction<GameObject, int> showDamageTakenFunction;


    //Keeping track since we access some objects on destroy, which causes null references if the application is also quitting

    bool quitting = false;
    void Awake()
    {
        fightManager = FindObjectOfType<FightManager>();
        bossFightManager = FindObjectOfType<BossFightManager>();
        playerQuests = FindObjectOfType<PlayerQuests>();


    }

    void OnApplicationQuit()
    {
        quitting = true;
    }

    void OnDestroy()
    {

        //PlayerPawn pPawn = FindObjectOfType<Player>().GetPlayerPawn();
        //pPawn.GetComponent<Unit>().dealtDamageEvent.RemoveListener(ShowDamageNumber);
        //pPawn.GetComponent<Health>().tookDamageEvent.RemoveListener(showDamageFunction);
        if (quitting == true)
            return;
       
        playerPawn.GetComponent<Unit>().dealtDamageEvent.RemoveListener(showDamageDealtFunction);


        playerPawn.GetComponent<Health>().tookDamageEvent.RemoveListener(showDamageTakenFunction);

    }
    


	// Should be called by fight manager when ready. If done in Start(), the order will differ from those of the NetworkBehaviours and
    //null references can occur
	public void Init (int areaNumber)
    {
        fightManager = FindObjectOfType<FightManager>();

        if (fightManager != null)
        {
            fightManager.enemySpawnedEvent.AddListener(UpdateUIForEnemy);
            areaText.text = fightManager.currentArea.areaName;
        }


        bossFightManager = FindObjectOfType<BossFightManager>();

        if (bossFightManager != null)
        {
            bossFightManager.bossSpawnedEvent.AddListener(UpdateUIForEnemy);
        }
            
        playerHealthBar.SetTarget(FindObjectOfType<Player>().GetPlayerPawn().gameObject);
        playerManaBar.SetTarget(FindObjectOfType<Player>().GetPlayerPawn().gameObject);


        playerPawn = FindObjectOfType<Player>().GetPlayerPawn();


        
        showDamageTakenFunction = (GameObject inst, int dmg) =>
        {
            ShowDamageNumber(playerPawn.gameObject, dmg);
        };

        showDamageDealtFunction= (GameObject inst, int dmg) =>
        {
            ShowDamageNumber(inst, dmg);
        };

        playerPawn.GetComponent<Unit>().dealtDamageEvent.AddListener(showDamageDealtFunction);


        playerPawn.GetComponent<Health>().tookDamageEvent.AddListener(showDamageTakenFunction);

    }
	
    void ShowDamageNumber(GameObject target, int damage)
    {
        //Bug with not using find object of type on combat hud that I dont think should exist
        //Game freaks when going to a new area
        var txt = Instantiate(floatingTextPrefab, transform).GetComponent<FloatingText>();

        Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        var position = Camera.main.WorldToScreenPoint(target.transform.position + offset) ;

        txt.Float(position, damage.ToString());
    }
	
    void UpdateUIForEnemy(GameObject enemy)
    {
        var ebars = Instantiate(enemyBarsPrefab, transform);
        ebars.UpdateWithEnemy(enemy.GetComponent<Enemy>());
        //Enemy e = enemy.GetComponent<Enemy>();
        //enemyHealthBar.SetTarget(enemy);
       // enemyName.text = e.unitName + " - Lv. " + e.level;
       // enemyBars.SetActive(true);
    }
    
}
