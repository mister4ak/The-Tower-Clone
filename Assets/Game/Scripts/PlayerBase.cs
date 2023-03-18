using System.Collections.Generic;
using ScriptableObjects.Classes;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private DamageableObserver _damageableObserver;
        private readonly List<IDamageable> _damageables = new ();
        private StateMachine _stateMachine;

        public void Initialize()
        {
            _damageableObserver.OnEntered += OnDamageableEntered;
            _damageableObserver.OnExited += OnDamageableExited;
            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();
        }

        private void OnDamageableEntered(IDamageable damageable)
        {
            if (_damageables.Contains(damageable)) return;
            _damageables.Add(damageable);
            damageable.Died += OnDamageableDied;
        }

        private void OnDamageableDied(IDamageable damageable)
        {
            if (!_damageables.Contains(damageable)) return;
            RemoveDamageableFromList(damageable);
        }

        private void RemoveDamageableFromList(IDamageable damageable)
        {
            damageable.Died -= OnDamageableDied;
            _damageables.Remove(damageable);
        }

        private void OnDamageableExited(IDamageable damageable)
        {
            if (!_damageables.Contains(damageable)) return;
            RemoveDamageableFromList(damageable);
        }
    }
}