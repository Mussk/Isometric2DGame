using UnityEngine;

namespace Weapon
{
    public class ProjectileWeapon : BaseWeapon
    {   
        
        [SerializeField]
        private PrefabSpawner prefabSpawner;
        public override void Attack(Vector2 direction, Quaternion aimRotation)
        {
            if (!CanFire()) return;
            SetNextFireTime();

            var projectileObj = prefabSpawner.Spawn();
            projectileObj.transform.position = transform.position;
            projectileObj.transform.rotation = transform.rotation;

            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.Initialize(direction, aimRotation, data.damage, data.projectileSpeed);
        }
    }
}
