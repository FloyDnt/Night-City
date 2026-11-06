using UnityEngine;
using UnityEngine.Pool;

namespace NightCity.Gameplay
{
    /// <summary>
    /// Fires projectiles of one prefab and reuses them through a pool.
    /// </summary>
    public sealed class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Prefab of the projectiles this launcher fires.")]
        private Projectile _projectilePrefab;

        private ObjectPool<Projectile> _pool;

        /// <summary>
        /// Fires a projectile.
        /// </summary>
        /// <param name="position">World position the projectile starts from.</param>
        /// <param name="direction">Flight direction.</param>
        /// <param name="team">Team of the shooter. Only units of the other team are damaged.</param>
        public void Launch(Vector2 position, Vector2 direction, Team team)
        {
            if (_pool == null)
            {
                return;
            }

            _pool.Get().Launch(position, direction, team);
        }

        /// <summary>
        /// Returns a projectile whose flight has ended to the pool.
        /// </summary>
        /// <param name="projectile">A projectile this launcher created.</param>
        internal void Release(Projectile projectile)
        {
            _pool.Release(projectile);
        }

        private void Awake()
        {
            if (_projectilePrefab == null)
            {
                Debug.LogError($"{nameof(ProjectileLauncher)} on '{name}' has no projectile prefab.", this);
                enabled = false;
                return;
            }

            _pool = new ObjectPool<Projectile>(CreateProjectile, TakeFromPool, ReturnToPool, DestroyProjectile);
        }

        private void OnDestroy()
        {
            if (_pool != null)
            {
                _pool.Clear();
            }
        }

        private Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(
                _projectilePrefab, transform.position, _projectilePrefab.transform.rotation);
            projectile.SetLauncher(this);
            return projectile;
        }

        private static void TakeFromPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private static void ReturnToPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        private static void DestroyProjectile(Projectile projectile)
        {
            // Idle projectiles are already gone when the whole scene is being unloaded.
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }
    }
}
