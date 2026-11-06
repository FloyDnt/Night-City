using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// A monster that walks back and forth, turning around at anything in its way except the character.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PatrollingMonster : Monster
    {
        // The probe sits ahead of the monster at body height. World units; x is mirrored with the walking direction.
        private const float ProbeAhead = 0.6f;
        private const float ProbeHeight = 0.5f;
        private const float ProbeRadius = 0.2f;

        [SerializeField, Min(0f)]
        [Tooltip("Walking speed in units per second.")]
        private float _speed = 2f;

        [SerializeField]
        [Tooltip("Renderer flipped to face the walking direction.")]
        private SpriteRenderer _sprite;

        private readonly Collider2D[] _probeHits = new Collider2D[16];
        private ContactFilter2D _probeFilter;
        private Rigidbody2D _body;

        // +1 walks right, -1 walks left.
        private float _direction = 1f;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();

            // Triggers count as obstacles, so the monster also turns at hazards.
            _probeFilter = ContactFilter2D.noFilter;

            if (_sprite == null)
            {
                Debug.LogError($"{nameof(PatrollingMonster)} on '{name}' has no sprite renderer.", this);
                enabled = false;
                return;
            }

            _sprite.flipX = false;
        }

        private void FixedUpdate()
        {
            Vector2 probe = _body.position + new Vector2(ProbeAhead * _direction, ProbeHeight);
            int count = Physics2D.OverlapCircle(probe, ProbeRadius, _probeFilter, _probeHits);
            if (count > 0 && !ContainsCharacter(count))
            {
                _direction = -_direction;
                _sprite.flipX = _direction < 0f;
            }

            _body.MovePosition(_body.position + new Vector2(_direction * _speed * Time.fixedDeltaTime, 0f));
        }

        private bool ContainsCharacter(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_probeHits[i].TryGetComponent(out Character _))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
