using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beastiary : MonoBehaviour {

    public GameObject beastiaryPanel;
    public BeastiaryEntry beastiaryEntryPrefab;
    public BeastiaryDetailsPanel beastDetailPanel;

    public Enemy[] enemies;

    public void Start()
    {
        foreach(Enemy e in enemies)
        {
            var entry = Instantiate(beastiaryEntryPrefab, beastiaryPanel.transform);
            entry.targetEnemy = e;
        }
    }
}
