using System;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IDamageable
    {
        public event Action<IDamageable> Died;
        GameObject GameObject();
        void TakeDamage(float damage);
        bool IsActive { get; set; }
    }
}