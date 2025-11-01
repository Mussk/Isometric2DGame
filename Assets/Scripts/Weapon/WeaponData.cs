using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string weaponName;
        public int damage = 10;
        public float fireRate = 0.5f;
        public float projectileSpeed = 10f;
        public GameObject projectilePrefab;
        public bool isAutomatic;
    }
}

