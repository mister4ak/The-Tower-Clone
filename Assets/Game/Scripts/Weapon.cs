using QFSW.MOP2;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
        
    private ObjectPool _bulletsPool;
    private Transform _shotPoint;

    public void Initialize(Transform shotPoint)
    {
        _shotPoint = shotPoint; 
        _bulletsPool = ObjectPool.Create(_bulletPrefab.gameObject);
    }

    public void Shot(Transform target, float damage)
    {
        var shotPosition = _shotPoint.position;
        var bullet = _bulletsPool.GetObjectComponent<Bullet>(shotPosition);
        var direction = (target.position - shotPosition).normalized;
        bullet.Initialize(direction, damage, _bulletsPool);
    }
}