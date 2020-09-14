using System.Collections;
using UnityEngine;
namespace Unexpected.Enemy.Movement
{
    public class DirectToPlayer : MonoBehaviour, IMovement
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [Header("Properties")]
        [SerializeField] private Rigidbody2D _rigidbody2d;
        [SerializeField] private Collider2D _aggroRange;
        [Header("Attack Routine")]
        [SerializeField] private int _speed;
        [SerializeField] private float _timeToAttack;
        [SerializeField] private float _rotationSpeed;
        [Range(0, 1f)]
        [SerializeField] private float _movementSmoothing;
        [Space]
        [SerializeField] private Animator _animator;
        [Header("Miniboss")]
        [SerializeField] private bool _flyImmediate;
#pragma warning restore CS0649
        #endregion

        private enum Movement
        {
            None,
            Attacking,
            Aiming,
            Fired,
            Dead
        }
        private Movement _state;
        private Vector3 _velocity = Vector3.zero;
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_flyImmediate)
            {
                _state = Movement.Fired;
                _rigidbody2d.constraints = RigidbodyConstraints2D.None;
            }
            else
                _state = Movement.None;
        }

        public void Move()
        {
            switch (_state)
            {
                case Movement.Attacking:
                    StartCoroutine(TargetPlayer());
                    _state = Movement.Aiming;
                    break;
                case Movement.Fired:
                    _animator.SetBool("isCharging", true);
                    Vector3 targetVelocity =
                        (Vector2)transform.right * Time.fixedDeltaTime * _speed;
                    _rigidbody2d.velocity = Vector3.SmoothDamp(
                        _rigidbody2d.velocity,
                        targetVelocity,
                        ref _velocity,
                        _movementSmoothing);
                    break;
                default:
                    break;
            }
        }

        public void Die() => _state = Movement.Dead;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != _player || _state != Movement.None)
                return;
            _state = Movement.Attacking;
        }

        private IEnumerator TargetPlayer()
        {
            float time = 0f;

            while (time < _timeToAttack)
            {
                if (PauseTime.Paused)
                    yield return null;
                else
                {
                    float angle = Mathf.Atan2(
                _player.transform.position.y
                - transform.position.y,
                _player.transform.position.x
                - transform.position.x)
                * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.Euler(
                        new Vector3(0, 0, angle));
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        _rotationSpeed * Time.deltaTime);
                    time += Time.fixedDeltaTime;
                    yield return null;
                }
            }
            _rigidbody2d.constraints = RigidbodyConstraints2D.None;
            _state = Movement.Fired;
            yield return null;
        }
    }
}

/* The enemy will remain stationary until the player enters thier
 * aggro range, at which point they will track the player and
 * launch themselves in a straight line, flying off the screen. */