using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMemoryPoool : MonoBehaviour
{
    [SerializeField]
    private Transform target; //적의 목표(플레이어)
    [SerializeField]
    private GameObject enemySpawnPointPrefab; //적이 등장하기 전 적의 등장 위치를 알려주는 프리팹
    [SerializeField]
    private GameObject enemyPrefab; //생성되는 적 프리펩
    [SerializeField]
    private float enemySpawnTime = 1; //적 생성 주기
    [SerializeField]
    private float enemySpawnLatency = 1; //타입 생성 후 적이 등장하기전까지의 대기시간

    private MemoryPool spawnPointMemoryPool; //적 등장 위치를 알려주는 오브젝트 생성, 활성/비활성 관리
    private MemoryPool enemyMemoryPool; //적 생성, 활성/비활성 관리

    private int numberOfEnemiesSpawnedAtOnce = 1;
    private Vector2 mapSize = new Vector2Int(15,15);

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPool = new MemoryPool(enemyPrefab);

        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        int currentNumber = 0;
       // int maximumNumber = 50; //원본 크기
        int maximumNumber = 10;

        while(true)
        {
          //동시에 numberOfEnemiesSpawnedAtOnce 숫자만큼 적이 생성되도록 반복문 생성 
          for(int i =0; i < numberOfEnemiesSpawnedAtOnce; ++i)
            {
                GameObject item = spawnPointMemoryPool.ActivePoolItem();

                item.transform.position = new Vector3(Random.Range(-mapSize.x * 0.49f, mapSize.x * 0.49f), 1,
                    Random.Range(-mapSize.y * 0.49f, mapSize.y * 0.49f));

                StartCoroutine("SpawnEnemy",item);
            }
            currentNumber++;

            if(currentNumber >= maximumNumber)
            {
                currentNumber = 0;
                numberOfEnemiesSpawnedAtOnce++;
            }
            yield return new WaitForSeconds(enemySpawnTime);       
        }
    }

    private IEnumerator SpawnEnemy(GameObject point)
    {
            yield return new WaitForSeconds(enemySpawnLatency);

            //적 오브젝트를 생성하고, 적의 위치를 point의 위치로 설정
            GameObject item = enemyMemoryPool.ActivePoolItem();
            item.transform.position = point.transform.position;

            item.GetComponent<EnemyFSM>().SetUp(target, this);

            //타일 오브젝트를 재활성화
            spawnPointMemoryPool.DeactivePoolItem(point); 
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivePoolItem(enemy);    
    }

    public void EveryStart()
    {
        enemyMemoryPool.ActivePoolItem();
    }

    public void EveryStop()
    {
        enemyMemoryPool.DeactiveAllPoolItems();
    }
}
