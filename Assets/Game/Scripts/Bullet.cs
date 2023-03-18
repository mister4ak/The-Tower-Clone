using Interfaces;
using QFSW.MOP2;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        
        private float _damage;
        private Vector2 _direction;
        private ObjectPool _bulletsPool;
        private bool _isActive;

        public void Initialize(Vector2 direction, float damage, ObjectPool bulletsPool)
        {
            _bulletsPool = bulletsPool;
            _direction = direction;
            _damage = damage;
            _isActive = true;
        }

        private void Update()
        {
            if (!_isActive) return;
            Move();
        }

        private void Move()
        {
            transform.Translate(_direction * _speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable)) return;
            damageable.TakeRealDamage(_damage);
            _isActive = false;
            _bulletsPool.Release(gameObject);
        }
    }
}