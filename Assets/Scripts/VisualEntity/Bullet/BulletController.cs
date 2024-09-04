using UnityEngine;

public class BulletController : VisualEntityController
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform bulletsContainer;

    public void PlaceBullet(int x, int y)
    {
        PlaceVisualEntity(bullet, x, y, bulletsContainer);
    }
}