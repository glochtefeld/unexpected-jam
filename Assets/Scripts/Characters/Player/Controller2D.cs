using UnityEngine;
using UnityEngine.Events;

namespace Unexpected.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Controller2D : MonoBehaviour
    {
        [Header("Movement Variables")]
        public float jumpForce = 400f;
        [Range(0, 1)]
        public float crouchSpeedMultiplier = 0.4f;
        [Range(0, 0.3f)]
        public float movementSmoothing = 0.05f;
        public bool airControl = true;
        [Header("Collision Bounding")]
        public LayerMask ground;
        public Transform groundCheck;
        public Transform ceilingCheck;
        public Collider2D crouchDisableCollider;
        [Header("Events")]
        [Space]
        public UnityEvent onLandEvent;
        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> { }
        public BoolEvent onCrouchEvent;
        private bool _isCrouching = false;

        private const float GROUNDED_RADIUS = 0.2f;
        private const float CEILING_RADIUS = 0.2f;
        private bool _isGrounded;
        private bool _facingRight = true;
        private Rigidbody2D _rigidbody2d;
        private Vector3 _velocity = Vector3.zero;

        #region Monobehaviour
        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            if (onLandEvent == null)
                onLandEvent = new UnityEvent();
            if (onCrouchEvent == null)
                onCrouchEvent = new BoolEvent();
        }

        private void FixedUpdate()
        {
            bool wasGrounded = _isGrounded;
            _isGrounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                groundCheck.position,
                GROUNDED_RADIUS,
                ground);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    _isGrounded = true;
                    if (!wasGrounded)
                        onLandEvent.Invoke();
                }
            }
        }
        #endregion

        public void Move(float move, bool crouch, bool jump)
        {
            if (!crouch
                && Physics2D.OverlapCircle(
                    ceilingCheck.position,
                    CEILING_RADIUS,
                    ground))
                crouch = true;

            if (_isGrounded || airControl)
            {
                if (crouch)
                {
                    if (!_isCrouching)
                    {
                        _isCrouching = true;
                        onCrouchEvent.Invoke(true);
                    }

                    move *= crouchSpeedMultiplier;

                    if (crouchDisableCollider != null)
                        crouchDisableCollider.enabled = false;
                }
                else
                {
                    if (crouchDisableCollider != null)
                        crouchDisableCollider.enabled = true;
                    if (_isCrouching)
                    {
                        _isCrouching = false;
                        onCrouchEvent.Invoke(false);
                    }
                }

                Vector3 targetVelocity = new Vector2(move * 10f,
                    _rigidbody2d.velocity.y);

                _rigidbody2d.velocity = Vector3.SmoothDamp(
                    _rigidbody2d.velocity,
                    targetVelocity,
                    ref _velocity,
                    movementSmoothing);

                if ((move > 0 && !_facingRight)
                    || (move < 0 && _facingRight))
                    Flip();
            }

            if (_isGrounded && jump)
            {
                _isGrounded = false;
                _rigidbody2d.AddForce(new Vector2(0f, jumpForce));
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
}