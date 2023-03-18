using System;
using DefaultNamespace;
using ScriptableObjects.Classes;
using UnityEngine;
using static Common.Enums;

namespace Enemy
{
    public class EnemyBasic : MonoBehaviour, IDamageable
    {
        public event Action<IDamageable> Died;

        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private EnemyMover _enemyMover;
        private float _health;

        public GameObject GameObject() => gameObject;
        public EnemyType Type => _enemyData.Type;
        public bool IsActive { get; set; }
        public int Reward => _enemyData.Reward;

        public void Initialize(Vector2 playerPosition)
        {
            _health = _enemyData.Health;
            _enemyMover.Initialize(_enemyData.Speed);
            _enemyMover.MoveTo(playerPosition);
            IsActive = true;
        }
        
        public void TakeDamage(float damage)
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
            Died?.Invoke(this);
        }
    }
}