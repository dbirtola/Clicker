using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMonstersQuest : Quest {

    public Enemy targetEnemy;
    public int progress = 0;
    public int required = 20;



    public override void StartQuest()
    {
        base.StartQuest();

        playerController.killedEnemyEvent.AddListener(OnEnemyKilled);
    }

    public void OnEnemyKilled(Enemy enemy)
    {
        Debug.Log("Checking type");
        Debug.Log("Types: " + enemy.GetType() + " vs " + targetEnemy.GetType());
        if (enemy.GetType() == targetEnemy.GetType()) {
            progress++;
            Debug.Log(questName + ": (" + progress + "/" + required +")");

            if(progress == required)
            {
                playerController.killedEnemyEvent.RemoveListener(OnEnemyKilled);

                FinishQuest();
            }
        }
    }
    
}
