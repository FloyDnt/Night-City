using System;
using UnityEngine;

namespace NightCity.Gameplay
{
    /// <summary>
    /// The player's character: reads input, runs, jumps, shoots and keeps track of its lives.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
    [RequireComponent(typeof(ProjectileLauncher))]
    public sealed class Character : Unit
    {
        private const string HorizontalAxis = "Horizontal";
        private const string JumpButton = "Jump";
        private const string FireButton = "Fire1";

        // World units.
        private const float GroundCheckRadius = 0.3f;

        // Upward impulse applied when the character is hit, in newton-seconds.
        private const float KnockbackImpulse = 4f;

        private static readonly int StateParameter = Animator.StringToHash("State");

        [SerializeField, Min(0f)]
        [Tooltip("Running speed in units per second.")]
        private float _speed = 3f;

        [SerializeField, Min(1)]
        [Tooltip("Lives the character starts the level with.")]
        private int _maxLives = 5;

        [SerializeField, Min(0f)]
        [Tooltip("Upward impulse of a jump, in newton-seconds.")]
        private float _jumpImpulse = 15f;

        [SerializeField]
        [Tooltip("Point at the character's feet where the ground is checked.")]
        private Transform _groundCheck;

        private readonly Collider2D[] _groundHits = new Collider2D[8];
        private ContactFilter2D _groundFilter;
        private Rigidbody2D _body;
        private Animator _animator;
        private SpriteRenderer _sprite;
        private ProjectileLauncher _launcher;
        private int _lives;
        private float _horizontalInput;
        private bool _isRunning;
        private bool _jumpRequested;
        private bool _isGrounded;
        private MotionState _motionState = MotionState.Idle;

        /// <summary>
        /// Raised after the character loses a life, with the number of lives left.
        /// </summary>
        public event Action<int> LivesChanged;

        // Values of the animator's State parameter.
        private enum MotionState
        {
            Idle = 0,
            Run = 1,
            Jump = 2,
        }

        /// <summary>
        /// Lives the character has left.
        /// </summary>
        public int Lives => _lives;

        /// <inheritdoc />
        public override Team Team => Team.Player;

        /// <summary>
        /// Takes one life, knocks the character up and ends the game when no lives are left.
        /// </summary>
        public override void ReceiveDamage()
        {
            if (_lives <= 0)
            {
                return;
            }

            _lives--;
            LivesChanged?.Invoke(_lives);

            _body.linearVelocity = Vector2.zero;
            _body.AddForce(Vector2.up * KnockbackImpulse, ForceMode2D.Impulse);

            if (_lives == 0)
            {
                SceneLoader.Load(GameScene.GameOver);
            }
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _launcher = GetComponent<ProjectileLauncher>();
            _lives = _maxLives;
            _groundFilter = new ContactFilter2D { useTriggers = false };

            if (_groundCheck == null)
            {
                Debug.LogError($"{nameof(Character)} on '{name}' has no ground check point.", this);
                enabled = false;
            }
        }

        private void Start()
        {
            _isGrounded = CheckGround();
        }

        private void Update()
        {
            if (Input.GetButtonDown(FireButton))
            {
                Shoot();
            }

            _isRunning = Input.GetButton(HorizontalAxis);
            // Raw input: the character reaches full speed on the first frame instead of ramping up with the
            // axis sensitivity, and holding both directions cancels out to standing still.
            _horizontalInput = _isRunning ? Input.GetAxisRaw(HorizontalAxis) : 0f;
            if (_isRunning)
            {
                _sprite.flipX = _horizontalInput < 0f;
            }

            if (Input.GetButtonDown(JumpButton))
            {
                _jumpRequested = true;
            }

            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            _isGrounded = CheckGround();
            _body.linearVelocityX = _horizontalInput * _speed;

            if (_jumpRequested)
            {
                _jumpRequested = false;
                if (_isGrounded)
                {
                    _body.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out Monster _))
            {
                ReceiveDamage();
            }
        }

        private void Shoot()
        {
            Vector2 direction = _sprite.flipX ? Vector2.left : Vector2.right;
            _launcher.Launch(transform.position, direction, Team);
        }

        private bool CheckGround()
        {
            int count = Physics2D.OverlapCircle(_groundCheck.position, GroundCheckRadius, _groundFilter, _groundHits);
            for (int i = 0; i < count; i++)
            {
                if (_groundHits[i].attachedRigidbody != _body)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateAnimation()
        {
            MotionState state;
            if (!_isGrounded)
            {
                state = MotionState.Jump;
            }
            else if (_isRunning)
            {
                state = MotionState.Run;
            }
            else
            {
                state = MotionState.Idle;
            }

            if (state != _motionState)
            {
                _motionState = state;
                _animator.SetInteger(StateParameter, (int)state);
            }
        }
    }
}
