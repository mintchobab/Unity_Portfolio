namespace lsy 
{
    public interface IDamageable
    {
        public void TakeDamage(int damage);
        public bool CheckIsDead();
    }
}
