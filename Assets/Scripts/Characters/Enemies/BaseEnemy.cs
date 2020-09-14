using System.Collections;
using Unexpected.Enemy.Movement;
using UnityEngine;

namespace Unexpected.Enemy
{
    [RequireComponent(typeof(IMovement))]
    public class BaseEnemy : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _mainCollider;
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _ps;
#pragma warning restore CS0649
        #endregion

        private bool _dead = false;
        private bool _isPaused = false;
        private IMovement _movement;
        private Vector3 _velocity;

        #region Monobehaviour
        private void Awake()
        {
            _movement = GetComponent<IMovement>();
            // TODO: get the layermask version of PlayerWalls
            Physics2D.IgnoreLayerCollision(gameObject.layer, 12);
        }

        void FixedUpdate()
        {
            if (_dead)
                return;

            if (!PauseTime.Paused && !_isPaused)
                _movement.Move();
            else if (!PauseTime.Paused && _isPaused)
            {
                _rigidbody.constraints = 
                    RigidbodyConstraints2D.FreezeRotation;
                _isPaused = false;
                _animator.speed = 1;
            }
            else if (PauseTime.Paused && !_isPaused)
                StartCoroutine(FreezePosition());

            if (transform.position.y < -20)
                StartCoroutine(Die());
        }
        #endregion

        private IEnumerator FreezePosition()
        {
            _animator.speed = 0;
            _isPaused = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            Vector3.SmoothDamp(
                _rigidbody.velocity,
                Vector3.zero,
                ref _velocity,
                0.5f);
            yield return null;
        }

        #region Health and Damage
        public IEnumerator Die()
        {
            var ps = Instantiate(_ps, transform.position, Quaternion.identity);
            ps.Play();
            
            if (!_dead)
            {
                _dead = true;
                _movement.Die();
                while (PauseTime.Paused)
                {
                    yield return null;
                }
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                _rigidbody.AddTorque(1, ForceMode2D.Impulse);
                _rigidbody.AddForce(Vector2.down, ForceMode2D.Impulse);
                _mainCollider.enabled = false;
                yield return new WaitForSeconds(1f);
                Destroy(gameObject);
            }
        }
        #endregion
    }
}
/* Wrapper script for all non-major enemies. Controls pausing,
 * dying, movement, etc. */