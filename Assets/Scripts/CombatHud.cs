using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public PlayerQuests playerQuests;
    public PlayerPawn playerPawn;

    public FloatingText floatingTextPrefab;

    //public GameObject enemyBars;

    void Awake()
    {
        fightManager = FindObjectOfType<FightManager>();
        bossFightManager = FindObjectOfType<BossFightManager>();
        playerQuests = FindObjectOfType<PlayerQuests>();
    }

    void Start()
    {
        playerQuests.questFinishedEvent.AddListener(ShowQuestFinishedPopup);
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
        FindObjectOfType<Player>().GetPlayerPawn().GetComponent<Unit>().dealtDamageEvent.AddListener(ShowDamageNumber);
	}
	
    void ShowDamageNumber(GameObject target, int damage)
    {
        var txt = Instantiate(floatingTextPrefab, FindObjectOfType<CombatHud>().transform).GetComponent<FloatingText>();
        var position = Camera.main.WorldToScreenPoint(target.transform.position);

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

    public void ShowQuestFinishedPopup(Quest quest)
    {

    }
}
