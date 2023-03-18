using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using QFSW.MOP2;
using ScriptableObjects.Classes;
using UnityEngine;
using static Common.Enums;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<EnemyBasic> _enemiesPrefab;
        [SerializeField] private SpawnZone _spawnZone;
        [SerializeField] private WaveData _waveData;
        private Dictionary<EnemyType, ObjectPool> _enemiesPooles = new();
        private float _totalWeight;
        private Vector2 _playerPosition;

        private void Start()
        {
            CalculateWeights();
            StartCoroutine(SpawnCoroutine());
        }

        public void Initialize(Vector2 playerPosition)
        {
            _playerPosition = playerPosition;
        }

        private void CalculateWeights()
        {
            foreach (var chanceData in _waveData.EnemyChanceDatas) 
                _totalWeight += chanceData.Probability;
        }

        private EnemyType GetRandomEnemyType()
        {
            float randomWeight = Random.value * _totalWeight;

            foreach (var chanceData in _waveData.EnemyChanceDatas)
            {
                if (randomWeight < chanceData.Probability)
                    return chanceData.Type;
                randomWeight -= chanceData.Probability;
            }

            return _waveData.EnemyChanceDatas[^1].Type;
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                var enemyType = GetRandomEnemyType();
                if (!_enemiesPooles.ContainsKey(enemyType))
                {
                    var enemyPrefab = _enemiesPrefab.Find(x => x.Type == enemyType);
                    if (enemyPrefab == default)
                        throw new NullReferenceException($"Enemy spawner doesn't contains enemy prefab with type - {enemyType}");
                    _enemiesPooles.Add(enemyType, ObjectPool.Create(enemyPrefab.gameObject));
                }

                var enemy = _enemiesPooles[enemyType].GetObjectComponent<EnemyBasic>(_spawnZone.GetRandomPoint());
                enemy.Initialize(_playerPosition);
                enemy.Died += OnEnemyDied;
                
                yield return new WaitForSeconds(1f);
            }
        }

        private void OnEnemyDied(IDamageable damageable)
        {
            if (!damageable.GameObject().TryGetComponent(out EnemyBasic enemy)) return;
            enemy.Died -= OnEnemyDied;
            _enemiesPooles[enemy.Type].Release(enemy.gameObject);
        }
    }
}