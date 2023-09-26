using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Wave", menuName = "Wave/Wave")]
public class WaveSO : ScriptableObject
{  
    [Header("Night: 소환할 적 리스트")]
    [Tooltip("소환할 적 SO와 마리 수")]
    public List<EnemySpawnInfo> enemySpawnList = new List<EnemySpawnInfo>();
    public int EnemySpawnCount
    {
        get
        {
            int cnt = 0;
            enemySpawnList.ForEach(x => cnt += x.count);
            return cnt;
        }
    }
    
}

[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyData eData;
    public int count;
}