using System;
using Interfaces;
using ScriptableObjects.Classes;
using UnityEngine;
using static Common.Enums;

namespace Enemy
{
    public class EnemyBasic : MonoBehaviour, IVirtualDamageable
    {
        public event Action<IVirtualDamageable> VirtualDied;
        public event Action<IDamageable> Died;

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
        
        public void StopMoving()
        {
            _enemyMover.StopMoving();
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
            VirtualDied?.Invoke(this);
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
            StopMoving();
            Died?.Invoke(this);
        }
    }
}