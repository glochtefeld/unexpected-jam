using Unexpected.Enemy.Movement;
using UnityEngine;
namespace Unexpected.Enemy.Movement
{
    public class BounceAtPlayer : MonoBehaviour, IMovement
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Rigidbody2D _rigidbody2d;
        [Header("Movement Variables")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _speed;
        [SerializeField] private Transform _groundCheck;
        [Range(0,1f)]
        [SerializeField] private float _movementSmoothing;
        [Space]
        [SerializeField] private LayerMask _ground;
        [Space]
        [SerializeField] private Transform _sprite;
#pragma warning restore CS0649
        #endregion

        private const float GROUND_CHECK_RADIUS = 0.2f;
        private GameObject _player;
        private bool _started = false;
        private Vector3 _velocity = Vector3.zero;
        #region Monobehaviour
        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != _player)
                return;
            _started = true;
            _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        #endregion

        #region IMovement
        public void Die() { }

        public void Move()
        {
            if (!_started)
                return;
            var direction = (
                _player.transform.position.x
                > transform.position.x)
                ? 1
                : -1;

            Vector3 targetVelocity = new Vector3(
                direction * Time.fixedDeltaTime * _speed,
                _rigidbody2d.velocity.y);

            _rigidbody2d.velocity = Vector3.SmoothDamp(
                _rigidbody2d.velocity,
                targetVelocity,
                ref _velocity,
                _movementSmoothing);

            _sprite.localScale = (_rigidbody2d.velocity.x <= 0f)
                ? new Vector3(-1f, 1f, 1f)
                : Vector3.one;

            if (Physics2D.OverlapCircle(
                _groundCheck.position,
                GROUND_CHECK_RADIUS,
                _ground) != null)
                Jump();

        }
        #endregion

        private void Jump() => 
            _rigidbody2d.AddForce(new Vector2(0f, _jumpForce));

    }
}