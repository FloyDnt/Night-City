using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// A shot that flies in a straight line, damages the first unit of the other team it touches and
    /// disappears after a fixed lifetime. It passes through everything else.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        [Tooltip("Flight speed in units per second.")]
        private float _speed = 10f;

        [SerializeField, Min(0f)]
        [Tooltip("Seconds the projectile flies before it disappears.")]
        private float _lifetime = 0.5f;

        private Rigidbody2D _body;
        private ProjectileLauncher _launcher;
        private Vector2 _velocity;
        private Team _team;
        private float _despawnTime;
        private bool _isFlying;

        /// <summary>
        /// Starts a flight.
        /// </summary>
        /// <param name="position">World position the flight starts from.</param>
        /// <param name="direction">Flight direction; only its orientation is used.</param>
        /// <param name="team">Team of the shooter. Only units of the other team are damaged.</param>
        public void Launch(Vector2 position, Vector2 direction, Team team)
        {
            // The z the launcher created the projectile at decides its draw order among sprites of equal order.
            transform.position = new Vector3(position.x, position.y, transform.position.z);
            _body.position = position;
            _velocity = direction.normalized * _speed;
            _team = team;
            _despawnTime = Time.time + _lifetime;
            _isFlying = true;
        }

        /// <summary>
        /// Sets the launcher whose pool this projectile returns to when its flight ends.
        /// </summary>
        /// <param name="launcher">The launcher that created this projectile.</param>
        internal void SetLauncher(ProjectileLauncher launcher)
        {
            _launcher = launcher;
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_isFlying && Time.time >= _despawnTime)
            {
                Despawn();
            }
        }

        private void FixedUpdate()
        {
            if (_isFlying)
            {
                _body.MovePosition(_body.position + _velocity * Time.fixedDeltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isFlying)
            {
                return;
            }

            if (other.TryGetComponent(out Unit unit) && unit.Team != _team)
            {
                unit.ReceiveDamage();
                Despawn();
            }
        }

        private void Despawn()
        {
            if (!_isFlying)
            {
                return;
            }

            _isFlying = false;

            // The launcher is gone when its owner was killed while this projectile was in flight.
            if (_launcher != null)
            {
                _launcher.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
