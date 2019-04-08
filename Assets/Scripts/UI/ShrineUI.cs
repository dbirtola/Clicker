using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrineUI : MonoBehaviour {

    public FightManager fightManager;


    public Slider tapsSlider;

    public Button runButton;

    Shrine currentShrine;

    void Start()
    {
        fightManager.shrineSpawnedEvent.AddListener(StartShrineUI);
    }

    void StartShrineUI(GameObject shrine)
    {
        currentShrine = shrine.GetComponent<Shrine>();
        tapsSlider.gameObject.SetActive(true);
        tapsSlider.maxValue = Shrine.maxExpectedTapSpeed;
        //currentShrine.shrineTappedEvent.AddListener(UpdateSlider);
        runButton.interactable = false;
        StartCoroutine(UpdateShrineUI());

    }

    IEnumerator UpdateShrineUI()
    {
        while (currentShrine.shrineActive)
        {
            UpdateSlider();
            yield return null;
        }

        tapsSlider.gameObject.SetActive(false);
        runButton.interactable = true;
    }

    void UpdateSlider()
    {
        tapsSlider.value = currentShrine.averageTapsPerSecond;
    }
}
