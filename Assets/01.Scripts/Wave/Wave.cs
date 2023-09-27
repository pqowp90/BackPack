using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Wave
{
    private List<Enemy> _enemies = new List<Enemy>();
    public WaveRange[] SpawnRanges;
    private WaveSO _waveInfo;
    public WaveSO WAVESO => _waveInfo;

    public int EnemyCount => _enemies.Count();

    public Wave(WaveSO sO)
    {
        _waveInfo = sO;

        InitializeEnemies();
        InitializeSpawnRanges();
    }

    private void InitializeEnemies()
    {
        foreach(var enemy in _waveInfo.enemySpawnList)
        {
            for(int i = 0; i < enemy.count; i++)
            {
                Enemy e = new Enemy();
                e.Init(enemy.eData);
                _enemies.Add(e);
            }
        }
        
        for(int i = 0; i < 100; i++)
        {
            int key = Random.Range(0, EnemyCount);
            Enemy e = _enemies[0];
            _enemies[0] = _enemies[key];
            _enemies[key] = e;
        }
    }

    private void InitializeSpawnRanges()
    {
        SpawnRanges = new WaveRange[3];

        if (WaveManager.Instance.WaveRanges.Clone() is not WaveRange[] ranges) return;
        var waveRanges = ranges.ToList();

        for (var i = 0; i < ranges.Length - 3; i++)
            waveRanges.RemoveAt(Random.Range(0, waveRanges.Count));

        for (var i = 0; i < 3; i++)
            SpawnRanges[i] = waveRanges[i];
    }

    public Enemy GetEnemy()
    {
        Enemy enemy = _enemies[0];
        _enemies.Remove(enemy);
        return enemy;
    }
}