using System;

namespace Interfaces
{
    public interface IVirtualDamageable : IDamageable
    {
        public event Action<IVirtualDamageable> VirtualDied;
        void TakeVirtualDamage(float damage);
        bool IsVirtualActive { get; set; }
    }
}