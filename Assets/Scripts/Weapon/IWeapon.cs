using UnityEngine;

namespace Weapon
{
    public interface IWeapon
    {
        void Initialize(Transform owner);
        void Attack(Vector2 direction, Quaternion rotation);
        void StopAttack(); // optional for automatic weapons
    }
}