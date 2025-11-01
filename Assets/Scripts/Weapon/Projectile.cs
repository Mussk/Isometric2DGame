using Enemy;
using UnityEngine;

namespace Weapon
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        private Vector2 direction;
        private float speed;
        private int damage;
        [SerializeField]
        private string projectileSpawnerTag = "ProjectileSpawner";
        private PrefabSpawner _prefabSpawner;

        [Header("Collision Tags")] [SerializeField]
        private string enemyTag = "Enemy";
        [SerializeField] private string obstacleTag = "Obstacle";
        
        private void Awake()
        {
            _prefabSpawner = GameObject.FindWithTag(projectileSpawnerTag).GetComponent<PrefabSpawner>();
        }
        
        public void Initialize(Vector2 dir, Quaternion rot, int dmg, float spd)
        {
            direction = dir.normalized;
            transform.rotation = rot;
            damage = dmg;
            speed = spd;
            
        }

        void Update()
        {
            transform.Translate(direction * (speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(enemyTag))
            {
                other.GetComponent<BaseEnemyController>()?.HealthSystem.TakeDamage(damage);
                _prefabSpawner.Despawn(gameObject);
            }

            if (other.CompareTag(obstacleTag))
            {
                _prefabSpawner.Despawn(gameObject);
                
            }
        }

        

        public void OnReuse()
        {
            transform.position = Vector3.zero;
        }
    }
}

