﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour, IEntity
{
    public Collider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene prev, LoadSceneMode scene)
    {
        OnEnable();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        OnDisable();
    }

    private void OnEnable()
    {
        if (EntityManager.Instance != null)
            EntityManager.Instance.RegisterEntity(this);
    }

    private void OnDisable()
    {
        if (EntityManager.Instance != null)
            EntityManager.Instance.UnregisterEntity(this);
    }

    [field: SerializeField] public EntityType Type { get; set; }
}

public interface IDamageable : IHealth
{
    public UnityEvent<float> OnDamageTaken { get; set; }
    public void TakeDamage(float damage);
}

public interface IHealth
{
    public float Health { get; set; }
}