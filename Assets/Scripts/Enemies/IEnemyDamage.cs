using System;

namespace Enemies
{
    // an interfaces to be implemented by objects that can take damage.
    public interface IEnemyDamage
    {
        public void TakeDamage(float damage);

    }
}
