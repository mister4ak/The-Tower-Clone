using System;

namespace Interfaces
{
    public interface IVirtualDamageable : IDamageable
    {
        public event Action<IVirtualDamageable> OnVirtualDied;
        void TakeVirtualDamage(float damage);
        bool IsVirtualActive { get; set; }
    }
}