using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyObjects;
    public int xPosition;
    public int zPosition;
    public int enemyCount;

    private EnemyAI enemies;

    private void Start()
    {
        enemies = new EnemyAI();
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while(enemyCount < 10)
        {
            xPosition = Random.Range(1, 30);
            zPosition = Random.Range(1, 31);
            Instantiate(enemyObjects, new Vector3(xPosition, 0, zPosition), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
      
            if(enemies.isDead == true)
            {
                Destroy(enemyObjects);
                enemyCount -= 1;
            }

        }
    }

}
