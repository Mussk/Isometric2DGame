using UnityEngine;

namespace Weapon
{
    public abstract class BaseWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] public WeaponData data;
        protected Transform owner;
        protected float nextFireTime;

        public virtual void Initialize(Transform owner)
        {
            this.owner = owner;
        }

        public abstract void Attack(Vector2 direction, Quaternion rotation);

        public virtual void StopAttack() { }

        protected bool CanFire()
        {
            return Time.time >= nextFireTime;
        }

        protected void SetNextFireTime()
        {
            nextFireTime = Time.time + data.fireRate;
        }
    }
}
