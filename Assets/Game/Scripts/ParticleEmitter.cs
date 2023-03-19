using System;
using System.Collections.Generic;
using Common;
using QFSW.MOP2;
using UnityEngine;
using Utils;

public class ParticleEmitter : MonoBehaviour
{
    [SerializeField] private List<EnemyParticleData> _particleDatas;
        
    private readonly Dictionary<Enums.EnemyType, ObjectPool> _particlePooles = new();
        
    public void PlayDieEnemy(Enums.EnemyType enemyType, Vector2 position)
    {
        if (!_particlePooles.ContainsKey(enemyType)) 
            CreateObjectPool(enemyType);

        var particlePool = _particlePooles[enemyType];
        var particle = particlePool.GetObjectComponent<ParticleSystem>();
            
        particle.transform.position = position;
        particle.Play();
            
        StartCoroutine(Helper.WaitCoroutine(3f, () => particlePool.Release(particle.gameObject)));
    }

    private void CreateObjectPool(Enums.EnemyType enemyType)
    {
        var particleData = _particleDatas.Find(x => x.Type == enemyType);
        if (particleData.ParticlePrefab == default)
            throw new NullReferenceException($"Particle Emitter doesn't contains particle prefab for type - {enemyType}");
            
        var objectPool = ObjectPool.Create(particleData.ParticlePrefab.gameObject);
        objectPool.ObjectParent.SetParent(transform);
            
        _particlePooles.Add(enemyType, objectPool);
    }
}

[Serializable]
public class EnemyParticleData
{
    public Enums.EnemyType Type;
    public ParticleSystem ParticlePrefab;
}