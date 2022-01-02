using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{

    public int monsterKillCount = 0;

    void Update()
    {
        monsterKillCount = DataController.Instance.gameData.enemyKillCount;
    }
}
