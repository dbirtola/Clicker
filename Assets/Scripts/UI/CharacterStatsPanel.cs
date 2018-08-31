using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsPanel : MonoBehaviour {


    Player player;
    PlayerInventory playerInventory;

    public Text levelText;
    public Text damageText;
    public Text healthText;
    public Text armorText;
    public Text criticalChanceText;
    public Text criticalDamageText;
    public Text manaPerTapText;
    public Text lifeStealText;
    public Text poisonDamageText;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerInventory.itemEquippedEvent.AddListener(Refresh);

        FindObjectOfType<PersistanceManager>().persistantDataLoadedEvent.AddListener(() => { Refresh(null); });
       // Refresh(null);
	}
	
	public void Refresh(Item item)
    {

        PlayerStatStruct stats = player.GetComponent<PlayerStats>().GetTotalStatStruct();


        levelText.text = stats.level.ToString();
        damageText.text = stats.damage.ToString();
        healthText.text = stats.maxHealth.ToString();
        armorText.text = stats.armor.ToString() + " (-" + (int)(player.GetComponent<PlayerStats>().GetDamageReduction() * 100) + "%)";
        criticalChanceText.text = stats.criticalChance + "%";
        criticalDamageText.text = stats.criticalDamageBonus * 100 + "%";
        manaPerTapText.text = stats.manaPerTap.ToString();
        lifeStealText.text = stats.lifeSteal * 100 + "%";
        poisonDamageText.text = stats.poisonDamage + "x";
    }
}
