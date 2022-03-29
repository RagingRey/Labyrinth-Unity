namespace Assets.Scripts
{
    public interface IAllBulletsBehaviour
    {
        void OnShoot(float speed = 40f);
        void DestroyBullet(float lifeTime);
        void BulletCollision();
    }
}