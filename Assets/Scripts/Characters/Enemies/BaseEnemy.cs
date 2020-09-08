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
            }
            else if (PauseTime.Paused && !_isPaused)
                StartCoroutine(FreezePosition());
        }
        #endregion

        private IEnumerator FreezePosition()
        {
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
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Lives>() == null)
                return;
            // Foot position
            if (collision.transform.GetChild(2).position.y 
                > transform.position.y)
                StartCoroutine(Die());
            else if (!PauseTime.Paused)
                collision.gameObject.GetComponent<Lives>().LoseLife();
        }

        private IEnumerator Die()
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
            DestroyImmediate(gameObject);
        }
        #endregion
    }
}
/* Wrapper script for all non-major enemies. Controls pausing,
 * dying, movement, etc. */