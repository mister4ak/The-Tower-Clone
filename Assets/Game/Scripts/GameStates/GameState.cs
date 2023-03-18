using Enemy;
using Extensions;
using ScriptableObjects.Classes;
using UI;
using UnityEngine;
using Utils;

namespace GameStates
{
    public class GameState : IState
    {
        private EnemySpawner _enemySpawner;
        private WaveData _waveData;
        private GameUI _gameUI;
        private float _spawnTimer;
        private PlayerBase _playerBase;
        
        public bool IsGameLoosed { get; private set; }

        public GameState(EnemySpawner enemySpawner, PlayerBase playerBase, WaveData waveData, GameUI gameUI)
        {
            _playerBase = playerBase;
            _gameUI = gameUI;
            _enemySpawner = enemySpawner;
            _waveData = waveData;
        }

        public void Tick()
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer > 0) return;
            _enemySpawner.SpawnEnemy();
            _spawnTimer = Random.Range(_waveData.MinDelayBeforeSpawn, _waveData.MaxDelayBeforeSpawn);
        }

        public void OnEnter()
        {
            _playerBase.StartGame();
            _enemySpawner.SetCurrentWaveData(_waveData);
            _playerBase.OnGameLoosed += OnGameLoosed;
        }

        private void OnGameLoosed()
        {
            IsGameLoosed = true;
        }

        public void OnExit()
        {
            _playerBase.OnGameLoosed -= OnGameLoosed;
            IsGameLoosed = false;
        }
    }
}