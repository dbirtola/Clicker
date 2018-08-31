using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Shrine : MonoBehaviour {

    public UnityEvent shrineTappedEvent;
    public UnityEvent shrineEventFinishedEvent;

    public const float maxExpectedTapSpeed = 15f;
    public const float shrineDuration = 5f;
    
    int experienceBonus = 1;
    int experienceBonusDuration = 10;

    public int cummulativeTaps;
    public float timeStarted;
    public float averageTapsPerSecond;

    public bool shrineActive = false;


    public void Awake()
    {
        shrineTappedEvent = new UnityEvent();
        shrineEventFinishedEvent = new UnityEvent();
    }

    public void StartShrineEvent()
    {
        timeStarted = Time.time;
        cummulativeTaps = 0;

        StartCoroutine(StartShrineRoutine());
    }

    IEnumerator StartShrineRoutine()
    {
        shrineActive = true;

        while(Time.time < timeStarted + shrineDuration)
        {
            averageTapsPerSecond = cummulativeTaps / (Time.time - timeStarted);
            yield return null;
        }


        shrineActive = false;
        CalculateResults();
        shrineEventFinishedEvent.Invoke();
    }

    public void OnMouseDown()
    {
        Debug.Log("Tapped");

        if (!shrineActive)
        {
            return;
        }

        cummulativeTaps++;


        shrineTappedEvent.Invoke();
    }

    

    public void CalculateResults()
    {
        for(int i = 0; i < 2; i++)
        {
            var charm = FindObjectOfType<ItemFactory>().GetCharmDrop();
            FindObjectOfType<PlayerInventory>().PickUpItem(charm);
        }
        for(int i = 0; i < 2; i++)
        {
            var area = FindObjectOfType<FightManager>().currentArea;
            var playerLevel = FindObjectOfType<PlayerStats>().GetTotalStatStruct().level;

            //Shrine item levels are the same as the player level, but max out at the areas max level
            FindObjectOfType<PlayerInventory>().PickUpItem(FindObjectOfType<ItemFactory>().GetItemDrop(Mathf.Min(playerLevel, area.maxLevel), true));
        }

        var buffs = FindObjectOfType<Player>().GetPlayerPawn().GetComponent<Buffs>();
        var newBuff = buffs.AddBuff<ExperienceBuff>();
        newBuff.expBonus = experienceBonus;
        newBuff.ActivateBuff(FindObjectOfType<Player>().GetPlayerPawn(), experienceBonusDuration);
        
        Debug.Log("Player tapped : " + cummulativeTaps + " (" + averageTapsPerSecond + " tps");
    }
    
    
}
