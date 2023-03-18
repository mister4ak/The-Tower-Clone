using System;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageableObserver : MonoBehaviour
    {
        public event Action<IDamageable> OnEntered;
        public event Action<IDamageable> OnExited;
        
        private Collider2D _collider;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void IncreaseRadius(float value)
        {
            transform.localScale += Vector3.one * value;
        }

        public void EnableCollider(bool enable) => _collider.enabled = enable;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable)) return;
            OnEntered?.Invoke(damageable);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable)) return;
            OnExited?.Invoke(damageable);
        }
    }
}