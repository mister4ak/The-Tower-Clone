using System;
using Interfaces;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageableObserver : MonoBehaviour
    {
        public event Action<IVirtualDamageable> OnEntered;
        public event Action<IVirtualDamageable> OnExited;
        
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
            if (!other.TryGetComponent(out IVirtualDamageable damageable)) return;
            OnEntered?.Invoke(damageable);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IVirtualDamageable damageable)) return;
            OnExited?.Invoke(damageable);
        }
    }
}