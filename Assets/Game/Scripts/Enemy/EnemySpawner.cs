using System;
using System.Collections.Generic;
using Interfaces;
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
        
        private Dictionary<EnemyType, ObjectPool> _enemiesPooles = new();
        private List<EnemyBasic> _enemies = new();
        private WaveData _waveData;
        private Vector2 _playerPosition;
        private float _totalWeight;

        public void Initialize(Vector2 playerPosition)
        {
            _playerPosition = playerPosition;
        }

        public void SetCurrentWaveData(WaveData waveData)
        {
            _waveData = waveData;
            CalculateWeights();
        }

        public void SpawnEnemy()
        {
            var enemyType = GetRandomEnemyType();
            if (!_enemiesPooles.ContainsKey(enemyType)) 
                CreateObjectPool(enemyType);

            InitializeEnemy(enemyType);
        }

        private void CreateObjectPool(EnemyType enemyType)
        {
            var enemyPrefab = _enemiesPrefab.Find(x => x.Type == enemyType);
            if (enemyPrefab == default)
                throw new NullReferenceException($"Enemy spawner doesn't contains enemy prefab with type - {enemyType}");
            _enemiesPooles.Add(enemyType, ObjectPool.Create(enemyPrefab.gameObject));
        }

        private void InitializeEnemy(EnemyType enemyType)
        {
            var enemy = _enemiesPooles[enemyType].GetObjectComponent<EnemyBasic>(_spawnZone.GetRandomPoint());
            enemy.Initialize(_playerPosition);
            enemy.OnDied += OnEnemyDied;
            _enemies.Add(enemy);
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

        private void OnEnemyDied(IDamageable damageable)
        {
            if (!damageable.GameObject().TryGetComponent(out EnemyBasic enemy)) return;
            enemy.OnDied -= OnEnemyDied;
            _enemies.Remove(enemy);
            _enemiesPooles[enemy.Type].Release(enemy.gameObject);
        }

        public void ReleaseAllEnemies()
        {
            foreach (var enemy in _enemies) 
                enemy.OnDied -= OnEnemyDied;
            _enemies.Clear();
            foreach (var enemiesPool in _enemiesPooles.Values) 
                enemiesPool.ReleaseAll();
        }
    }
}