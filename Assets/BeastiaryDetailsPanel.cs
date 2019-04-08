using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeastiaryDetailsPanel : MonoBehaviour {

    public Text enemyName;
    public Image enemyImage;
    public Text enemyDescription;
    public Text enemyMechanic;
    public Text suggestedStrategy;


    public void UpdateWithEnemy(Enemy enemy)
    {
        enemyName.text = enemy.unitName;
        enemyImage.sprite = enemy.GetComponent<SpriteRenderer>().sprite;
        enemyImage.color = enemy.GetComponent<SpriteRenderer>().color;
        enemyDescription.text = enemy.description;
        enemyMechanic.text = "\tMechanic: <color=black>" + enemy.mechanic + "</color>";
        suggestedStrategy.text = "\tSuggested Strategy: <color=black>" + enemy.counterplay + "</color>";


    }
}
