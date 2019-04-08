using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeastiaryEntry : MonoBehaviour {

    public Enemy targetEnemy;

    public Image image;
    public Text nameText;
    public Text descriptionText;

    void Start()
    {
        nameText.text = targetEnemy.name;
        descriptionText.text = targetEnemy.description;
        image.sprite = targetEnemy.GetComponent<SpriteRenderer>().sprite;
        image.color = targetEnemy.GetComponent<SpriteRenderer>().color;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            var beastPanel = FindObjectOfType<Beastiary>().beastDetailPanel;
            beastPanel.UpdateWithEnemy(targetEnemy);
            beastPanel.gameObject.SetActive(true);
        });
    }
}
