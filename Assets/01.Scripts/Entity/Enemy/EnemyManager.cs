using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public Transform enemyParent;
    public List<Enemy> Enemies = new();

    public void SpawnEnemy(Enemy enemy, Vector3 position)
    {
        var enemyObject = Instantiate(enemy);
        enemyObject.transform.position = position;
        enemyObject.transform.rotation = Quaternion.identity;
        enemyObject.transform.SetParent(enemyParent);
        enemyObject.Init(enemy.Data, true);
        Enemies.Add(enemyObject);
    }

    public void DeathEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        EntityManager.Instance.UnregisterEntity(enemy);
        Destroy(enemy);
    }
}