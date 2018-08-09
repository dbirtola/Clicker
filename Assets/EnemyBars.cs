using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBars : MonoBehaviour {

    public Enemy target;

    public HealthBar enemyHealthBar;
    public Text enemyName;

    public Vector3 offset;



    void Update()
    {
        if(target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position) + offset;
        }else
        {
            Destroy(gameObject);
        }

       
    }

    public void UpdateWithEnemy(Enemy enemy)
    {

        //Enemy e = enemy.GetComponent<Enemy>();
        target = enemy;
        enemyHealthBar.SetTarget(enemy.gameObject);
        enemyName.text = enemy.unitName + " - Lv. " + enemy.level;
        if (enemy.GetComponent<MagicEnemy>())
        {
            enemyName.color = Color.blue;
        }
       // enemy.unitDied.AddListener((Unit u) => { Destroy(gameObject); });
        //enemyBars.SetActive(true);
    }
}
