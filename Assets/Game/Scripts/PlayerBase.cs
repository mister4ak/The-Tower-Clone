using System;
using System.Collections.Generic;
using Common;
using Extensions;
using Interfaces;
using Providers;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public event Action OnGameLoosed;
    
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private DamageableObserver _damageableObserver;
    [SerializeField] private Weapon _weapon;
        
    private readonly List<IVirtualDamageable> _damageables = new();
    private float _attackCooldown;
    private float _attackTimer;
    private float _damage;
    public bool IsGameStarted { get; private set; }

    public void Initialize()
    {
        _weapon.Initialize(_shotPoint);

        InitializeUpgrades();

        _damageableObserver.OnEntered += OnDamageableEntered;
        _damageableObserver.OnExited += OnDamageableExited;
    }

    public void StartGame()
    {
        IsGameStarted = true;
    }

    private void InitializeUpgrades()
    {
        _damage = BoosterProvider.GetBoosterValue(Enums.UpgradeType.Damage);
        _attackCooldown = BoosterProvider.GetBoosterValue(Enums.UpgradeType.AttackCooldown);

        BoosterProvider.OnBoostersUpgraded += OnBoostersUpgraded;
    }
        
    private void OnDamageableEntered(IVirtualDamageable damageable)
    {
        if (_damageables.Contains(damageable)) return;
        _damageables.Add(damageable);
        damageable.OnVirtualDied += OnDamageableOnDied;
    }

    private void OnDamageableOnDied(IVirtualDamageable damageable)
    {
        RemoveDamageableFromList(damageable);
    }

    private void RemoveDamageableFromList(IVirtualDamageable damageable)
    {
        if (!_damageables.Contains(damageable)) return;
        damageable.OnVirtualDied -= OnDamageableOnDied;
        _damageables.Remove(damageable);
    }

    private void OnDamageableExited(IVirtualDamageable damageable)
    {
        RemoveDamageableFromList(damageable);
    }

    private void OnBoostersUpgraded(Enums.UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case Enums.UpgradeType.Damage:
                _damage = BoosterProvider.GetBoosterValue(Enums.UpgradeType.Damage);
                break;
            case Enums.UpgradeType.AttackCooldown:
                _attackCooldown = BoosterProvider.GetBoosterValue(Enums.UpgradeType.AttackCooldown);
                break;
            case Enums.UpgradeType.AttackRange:
                _damageableObserver.IncreaseRadius(BoosterProvider.GetBoosterMultiplierValue(Enums.UpgradeType.AttackRange));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(upgradeType), upgradeType, null);
        }
    }

    private void Update()
    {
        if (!IsGameStarted) return;
        TryAttack();
    }

    private void TryAttack()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer > 0f) return;
        if (_damageables.Count == 0) return;

        var target = FindNearestTarget();
        target.TakeVirtualDamage(_damage);
        _weapon.Shot(target.GameObject().transform, _damage);
            
        _attackTimer = _attackCooldown;
    }

    private IVirtualDamageable FindNearestTarget()
    {
        IVirtualDamageable nearestTarget = _damageables[0];
        if (_damageables.Count == 1)
            return _damageables[0];

        float minDistance = float.MaxValue;
        foreach (var damageable in _damageables)
        {
            if (!damageable.IsVirtualActive) continue;

            var distanceToTarget = transform.DistanceTo(damageable.GameObject().transform);
            if (distanceToTarget < minDistance)
            {
                nearestTarget = damageable;
                minDistance = distanceToTarget;
            }
        }

        return nearestTarget;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Enter in player" + other.gameObject.name);
        if (!other.gameObject.TryGetComponent(out IDamageable damageable)) return;
        LoseGame();
    }

    private void LoseGame()
    {
        IsGameStarted = false;
        OnGameLoosed?.Invoke();
    }
}