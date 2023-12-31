﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class WaveManager : MonoSingleton<WaveManager>
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private Vector3 waveSize = new(5f, 1f, 5f);

    [SerializeField] private float timer;
    [SerializeField] private float nextSpawnTime;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask _groundLayer;

    private readonly WaveRange[] _waveRanges = { new(), new(), new(), new() };

    public WaveRange[] WaveRanges => UpdateWaveRange();

    [field:SerializeField] public int WaveCount { get; private set; }
    public Wave CurrentWave { get; private set; }

    [Header("Wave Info")]
    public List<WaveSO> waveList = new List<WaveSO>();
    [SerializeField] private float _enemySpawnDelay;
    
    protected override void Start() 
    {
        base.Start();
        timer = 0f;
        nextSpawnTime = 0;
        WaveCount = 0;
        CurrentWave = new Wave(waveList[WaveCount]);
        _enemySpawnDelay = Random.Range(30.0f, 40.0f) / CurrentWave.WAVESO.EnemySpawnCount;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        UpdateWaveRange();
        if(CurrentWave != null) SpawnTimer();
    }

    private WaveRange[] UpdateWaveRange()
    {
        var position = player.position;

        _waveRanges[0].position = position + Vector3.forward * radius;
        _waveRanges[0].size = waveSize;
        _waveRanges[1].position = position + Vector3.back * radius;
        _waveRanges[1].size = waveSize;

        var reversed = new Vector3(waveSize.z, waveSize.y, waveSize.x);
        _waveRanges[2].position = position + Vector3.left * radius;
        _waveRanges[2].size = reversed;
        _waveRanges[3].position = position + Vector3.right * radius;
        _waveRanges[3].size = reversed;

        return _waveRanges;
    }

    public void StartWave(bool night)
    {
        timer = 0f;
        nextSpawnTime = 0;

        WaveCount++;
        if (waveList.Count <= WaveCount) WaveCount--;

        CurrentWave = new Wave(waveList[WaveCount]);

        _enemySpawnDelay = Random.Range(30.0f, 40.0f) / CurrentWave.WAVESO.EnemySpawnCount;
    }

    private void SpawnTimer()
    {
        if (nextSpawnTime > timer) return;
        if (CurrentWave.EnemyCount <= 0) return;

        SpawnEnemy();
        nextSpawnTime += _enemySpawnDelay;
    }

    private void SpawnEnemy()
    {
        var waveRange = CurrentWave.SpawnRanges[Random.Range(0, CurrentWave.SpawnRanges.Length)];
        var position = new Vector3(
            Random.Range(waveRange.position.x - waveRange.size.x / 2f,
                waveRange.position.x + waveRange.size.x / 2f),
            100f,
            Random.Range(waveRange.position.z - waveRange.size.z / 2f,
                waveRange.position.z + waveRange.size.z / 2f)
        );

        RaycastHit hit;
        Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity, _groundLayer);
        position = new Vector3(position.x, hit.point.y + 0.5f, position.z);

        EnemyManager.Instance.SpawnEnemy(CurrentWave.GetEnemy(), position);
    }
}