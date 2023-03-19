using System;
using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        public event Action<IDamageable> Died;
        GameObject GameObject();
        void TakeRealDamage(float damage);
        bool IsActive { get; set; }
    }
}