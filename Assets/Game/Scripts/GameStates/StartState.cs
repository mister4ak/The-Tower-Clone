using Enemy;
using UI;
using UnityEngine;
using Utils;

namespace GameStates
{
    public class StartState: IState
    {
        private GameUI _gameUI;
        private PlayerBase _player;
        private EnemySpawner _enemySpawner;
        public bool IsGameStarted { get; private set; }

        public StartState(GameUI gameUI, PlayerBase player, EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
            _player = player;
            _gameUI = gameUI;
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsGameStarted = true;
            }
        }

        public void OnEnter()
        {
            _gameUI.GameWindow.ShowMoveToPlay();
            _enemySpawner.ReleaseAllEnemies();
        }

        public void OnExit()
        {
            _gameUI.GameWindow.HideMoveToPlay();
            IsGameStarted = false;
        }
    }
}