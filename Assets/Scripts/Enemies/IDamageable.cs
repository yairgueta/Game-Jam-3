
// an interfaces to be implemented by objects that can take damage.
namespace Enemies
{
    public interface IDamageable
    {
        // reduces life according to the damage.
        public void TakeDamage(int damage);

        // returns true iff the damage taker lost all of it's life points.
        public bool IsDead();

        // gets the current amount of life points.
        public int GetLifePoints();
    }
}
