using System;
using System.Collections.Generic;
using Extensions;
using Interfaces;
using ScriptableObjects.Classes;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private DamageableObserver _damageableObserver;
        [SerializeField] private Weapon _weapon;
        
        private readonly List<IVirtualDamageable> _damageables = new();
        private float _attackCooldown;
        private float _attackTimer;
        private float _damage;

        public void Initialize()
        {
            _weapon.Initialize(_shotPoint);
            
            _damageableObserver.OnEntered += OnDamageableEntered;
            _damageableObserver.OnExited += OnDamageableExited;
        }

        private void Update()
        {
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

        private void OnDamageableEntered(IVirtualDamageable damageable)
        {
            if (_damageables.Contains(damageable)) return;
            _damageables.Add(damageable);
            damageable.OnVirtualDied += OnDamageableOnDied;
        }

        private void OnDamageableOnDied(IVirtualDamageable damageable)
        {
            if (!_damageables.Contains(damageable)) return;
            RemoveDamageableFromList(damageable);
        }

        private void RemoveDamageableFromList(IVirtualDamageable damageable)
        {
            damageable.OnVirtualDied -= OnDamageableOnDied;
            _damageables.Remove(damageable);
        }

        private void OnDamageableExited(IVirtualDamageable damageable)
        {
            if (!_damageables.Contains(damageable)) return;
            RemoveDamageableFromList(damageable);
        }
    }
}