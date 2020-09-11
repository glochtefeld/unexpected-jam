using Unexpected.Objects.Platforms.Types;
using UnityEngine;
using UnityEngine.Events;

namespace Unexpected.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Controller2D : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [Header("Movement Variables")]
        [SerializeField] private float _runSpeed = 40f;
        [SerializeField] private float _jumpVelocity = 400f;
        [Range(0, 1)]
        [SerializeField] private float _crouchSpeedMultiplier = 0.4f;
        [Range(0, 0.3f)]
        [SerializeField] private float _movementSmoothing = 0.05f;
        [SerializeField] private bool _airControl = true;
        [SerializeField] private float _jumpPressedMemoryTime = 0.2f;
        [SerializeField] private float _groundedMemoryTime = 0.25f;
        [Header("Collision Bounding")]
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Transform _ceilingCheck;
        [SerializeField] private Collider2D _crouchDisableCollider;
        [Space]
        [Header("Events")]
        [SerializeField] private UnityEvent _onLandEvent;
        [SerializeField] private BoolEvent _onCrouchEvent;
#pragma warning restore CS0649
        #endregion

        private const float GROUNDED_RADIUS = 0.2f;
        private const float CEILING_RADIUS = 0.2f;
        private float _jumpPressedMemory = 0f;
        private float _groundedMemory = 0f;
        private bool _isCrouching = false;
        private bool _isGrounded;
        private bool _facingRight = true;
        private Rigidbody2D _rigidbody2d;
        private Vector3 _velocity = Vector3.zero;
        private Collider2D[] _ceilingColliders;
        private Collider2D[] _groundColliders;

        #region Monobehaviour
        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            if (_onLandEvent == null)
                _onLandEvent = new UnityEvent();
            if (_onCrouchEvent == null)
                _onCrouchEvent = new BoolEvent();

        }

        private void FixedUpdate()
        {
            bool wasGrounded = _isGrounded;
            _isGrounded = false;

            _groundedMemory -= Time.fixedDeltaTime;
            _jumpPressedMemory -= Time.fixedDeltaTime;

            _groundColliders = Physics2D.OverlapCircleAll(
                _groundCheck.position,
                GROUNDED_RADIUS,
                _ground);
            for (int i = 0; i < _groundColliders.Length; i++)
            {
                if (_groundColliders[i].gameObject != gameObject
                    && !_groundColliders[i].isTrigger)
                {
                    _isGrounded = true;
                    _groundedMemory = _groundedMemoryTime;
                    if (!wasGrounded)
                        _onLandEvent.Invoke();
                }
            }
        }
        #endregion

        public void Move(float move, bool crouch, bool jump)
        {

            if (jump)
                _jumpPressedMemory = _jumpPressedMemoryTime;

            _ceilingColliders = Physics2D.OverlapCircleAll(
                _ceilingCheck.position,
                CEILING_RADIUS,
                _ground);

            // Trigger fallthrough, if available
            if (crouch)
            {
                foreach (var c in _groundColliders)
                {
                    if (c.GetComponent<Fallthrough>() != null)
                    {
                        StartCoroutine(c.
                            GetComponent<Fallthrough>()
                            .DisableCollider());
                        break;
                    }
                }
            }

            // Crouch if under a ceiling (unless it's a fallthrough)
            foreach (var collider in _ceilingColliders)
            {
                if (collider.GetComponent<Fallthrough>() != null)
                {
                    StartCoroutine(collider.
                        GetComponent<Fallthrough>()
                        .DisableCollider());
                    crouch = false;
                    break;
                }else if (!crouch && !collider.isTrigger)
                {
                    crouch = true;
                    break;
                }
            }

            // Reduce speed when crouching
            if (_isGrounded || _airControl)
            {
                if (crouch)
                {
                    if (!_isCrouching)
                    {
                        _isCrouching = true;
                        _onCrouchEvent.Invoke(true);
                    }

                    move *= _crouchSpeedMultiplier;

                    if (_crouchDisableCollider != null)
                        _crouchDisableCollider.enabled = false;
                }
                else
                {
                    if (_crouchDisableCollider != null)
                        _crouchDisableCollider.enabled = true;
                    if (_isCrouching)
                    {
                        _isCrouching = false;
                        _onCrouchEvent.Invoke(false);
                    }
                }

                // Move left or right
                Vector3 targetVelocity = new Vector2(move * _runSpeed,
                    _rigidbody2d.velocity.y);
                _rigidbody2d.velocity = Vector3.SmoothDamp(
                    _rigidbody2d.velocity,
                    targetVelocity,
                    ref _velocity,
                    _movementSmoothing);
                // Flip sprite
                if ((move > 0 && !_facingRight)
                    || (move < 0 && _facingRight))
                    Flip();
            }

            // Jumping
            if (_groundedMemory > 0
                && _jumpPressedMemory > 0
                && _rigidbody2d.velocity.y < 10f)
            {
                _isGrounded = false;
                _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _jumpVelocity);
                //_rigidbody2d.AddForce(new Vector2(0f, _jumpForce));
            }
        }

        private void Flip()
        {
            _facingRight = !_facingRight;
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

    }

    class BoolEvent : UnityEvent<bool> { }
}
/* Controller2D() takes normalized input from the player and applies
 * velocity to the character. It also handles calling any
 * fallthrough platforms.*/
/// <see cref="Fallthrough"/>
 /* The BoolEvent class is used as a child class by Controller2D.*/