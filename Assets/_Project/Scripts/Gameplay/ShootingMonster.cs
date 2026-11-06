using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// A stationary monster that turns towards its target and shoots at it at a fixed interval.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(ProjectileLauncher))]
    public sealed class ShootingMonster : Monster
    {
        [SerializeField, Min(0f)]
        [Tooltip("Seconds between shots, in scaled time. The first shot comes one interval after the level starts.")]
        private float _fireInterval = 2f;

        [SerializeField]
        [Tooltip("The transform the monster shoots at.")]
        private Transform _target;

        [SerializeField]
        [Tooltip("Where shots start relative to the monster, in world units, when it faces right. "
            + "Mirrored when it faces left.")]
        private Vector2 _muzzleOffset = new Vector2(0.7f, 0.2f);

        private SpriteRenderer _sprite;
        private ProjectileLauncher _launcher;
        private float _timeUntilShot;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _launcher = GetComponent<ProjectileLauncher>();
            _timeUntilShot = _fireInterval;

            if (_target == null)
            {
                Debug.LogError($"{nameof(ShootingMonster)} on '{name}' has no target.", this);
                enabled = false;
            }
        }

        private void Update()
        {
            _timeUntilShot -= Time.deltaTime;
            if (_timeUntilShot <= 0f)
            {
                _timeUntilShot += _fireInterval;
                Shoot();
            }
        }

        private void Shoot()
        {
            bool facesRight = _target.position.x > transform.position.x;

            // The sprite is drawn facing left.
            _sprite.flipX = facesRight;

            float side = facesRight ? 1f : -1f;
            Vector2 muzzle = (Vector2)transform.position + new Vector2(_muzzleOffset.x * side, _muzzleOffset.y);
            _launcher.Launch(muzzle, new Vector2(side, 0f), Team);
        }
    }
}
