using Common;
using Enemy;
using Extensions;
using Player;
using ScriptableObjects.Classes;
using UI;
using UI.ResourcesView;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace GameStates
{
    public class GameState : IState
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly WaveData _waveData;
        private readonly GameUI _gameUI;
        private readonly PlayerBase _playerBase;
        private readonly ParticleEmitter _particleEmitter;
        
        private float _spawnTimer;
        private float _waveTimer;
        private float _minSpawnDelay;
        private float _maxSpawnDelay;

        public bool IsGameLoosed { get; private set; }

        public GameState(EnemySpawner enemySpawner, PlayerBase playerBase, WaveData waveData, 
            GameUI gameUI, ParticleEmitter particleEmitter)
        {
            _particleEmitter = particleEmitter;
            _playerBase = playerBase;
            _gameUI = gameUI;
            _enemySpawner = enemySpawner;
            _waveData = waveData;
        }

        public void Tick()
        {
            TrySpawnEnemy();
            TryIncreaseDifficult();
        }

        private void TrySpawnEnemy()
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer > 0) return;
            
            _enemySpawner.SpawnEnemy();
            _spawnTimer = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        }

        private void TryIncreaseDifficult()
        {
            _waveTimer -= Time.deltaTime;
            if (_waveTimer > 0) return;
            
            DecreaseSpawnTime();
            _waveTimer = _waveData.WaveDuration;
        }

        private void DecreaseSpawnTime()
        {
            _minSpawnDelay -= _waveData.DecreaseSpawnSpeedPerWave;
            _maxSpawnDelay -= _waveData.DecreaseSpawnSpeedPerWave;
            
            _minSpawnDelay.Clamp(_waveData.MinSpawnTimeInGame, _waveData.StartMinSpawnTimer);
            _maxSpawnDelay.Clamp(_waveData.MinSpawnTimeInGame, _waveData.StartMaxSpawnTimer);
        }

        public void OnEnter()
        {
            _playerBase.StartGame();
            _playerBase.GameLoosed += OnGameLoosed;
            
            _enemySpawner.SetCurrentWaveData(_waveData);
            _enemySpawner.EnemyDied += OnEnemyDied;
            
            _gameUI.GameWindow.ShowBoosters();
            
            _minSpawnDelay = _waveData.StartMinSpawnTimer;
            _maxSpawnDelay = _waveData.StartMaxSpawnTimer;

            _waveTimer = _waveData.WaveDuration;
        }

        private void OnEnemyDied(EnemyBasic enemy)
        {
            var enemyPosition = enemy.transform.position;
            var enemyPositionOnScreen = Helper.Camera.WorldToScreenPoint(enemyPosition);
            
            ClaimAndShowReward(enemy, enemyPositionOnScreen);
            _particleEmitter.PlayDieEnemy(enemy.Type, enemyPosition);
        }

        private void ClaimAndShowReward(EnemyBasic enemy, Vector3 enemyPositionOnScreen)
        {
            ResourceHandler.AddResource(Enums.ResourceType.Money, enemy.Reward);
            _gameUI.GameWindow.ShowReward(enemyPositionOnScreen, enemy.Reward);
        }

        private void OnGameLoosed()
        {
            IsGameLoosed = true;
        }

        public void OnExit()
        {
            IsGameLoosed = false;
            _gameUI.GameWindow.HideBoosters();
            _enemySpawner.StopAllEnemies();
            _enemySpawner.EnemyDied -= OnEnemyDied;
            _playerBase.GameLoosed -= OnGameLoosed;
        }
    }
}