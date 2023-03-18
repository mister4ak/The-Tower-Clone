using System;
using Interfaces;
using ScriptableObjects.Classes;
using UnityEngine;
using static Common.Enums;

namespace Enemy
{
    public class EnemyBasic : MonoBehaviour, IVirtualDamageable
    {
        public event Action<IVirtualDamageable> OnVirtualDied;
        public event Action<IDamageable> OnDied;

        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private EnemyMover _enemyMover;
        
        private float _health;
        private float _virtualHealth;
        
        public GameObject GameObject() => gameObject;
        public EnemyType Type => _enemyData.Type;
        public int Reward => _enemyData.Reward;
        public bool IsActive { get; set; }
        public bool IsVirtualActive { get; set; }

        public void Initialize(Vector2 playerPosition)
        {
            _health = _virtualHealth = _enemyData.Health;
            
            _enemyMover.Initialize(_enemyData.Speed);
            _enemyMover.MoveTo(playerPosition);
            
            IsActive = true;
            IsVirtualActive = true;
        }
        
        public void TakeVirtualDamage(float damage)
        {
            _virtualHealth -= damage;
            if (_virtualHealth <= 0)
                VirtualDie();
        }

        private void VirtualDie()
        {
            if (!IsVirtualActive) return;
            IsVirtualActive = false;
            OnVirtualDied?.Invoke(this);
        }

        public void TakeRealDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
                Die();
        }

        private void Die()
        {
            if (!IsActive) return;
            IsActive = false;
            _enemyMover.StopMoving();
            OnDied?.Invoke(this);
        }
    }
}