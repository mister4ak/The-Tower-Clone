using System;
using Enemy;
using GameStates;
using Player;
using Providers;
using ScriptableObjects.Classes;
using UI;
using UnityEngine;
using Utils;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private ParticleEmitter _particleEmitter;
    [SerializeField] private PlayerBase _player;
    [SerializeField] private WaveData _waveData;

    private StateMachine _stateMachine;

    private void Start()
    {
        BoosterProvider.Instance.Initialize();
        _player.Initialize();
        _enemySpawner.Initialize(_player.transform.position);

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new StateMachine();

        var startState = new StartState(_gameUI, _enemySpawner);
        var gameState = new GameState(_enemySpawner, _player, _waveData, _gameUI, _particleEmitter);
        var loseState = new LoseState(_gameUI);

        _stateMachine.AddTransition(startState, gameState, IsGameStarted());
        _stateMachine.AddTransition(gameState, loseState, IsGameLoosed());
        _stateMachine.AddTransition(loseState, startState, IsRestartButtonClicked());

        Func<bool> IsGameLoosed() => () => gameState.IsGameLoosed;
        Func<bool> IsRestartButtonClicked() => () => loseState.IsRestartButtonClicked;
        Func<bool> IsGameStarted() => () => startState.IsGameStarted;

        _stateMachine.SetState(startState);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }
}