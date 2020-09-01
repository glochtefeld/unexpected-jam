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
        [SerializeField]private int _health;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _mainCollider;
#pragma warning restore CS0649
        #endregion

        private bool _dead = false;
        private bool _isPaused = false;
        private float _transitionTime = 0.5f;
        private IMovement _movement;

        #region Monobehaviour
        private void Awake()
        {
            _movement = GetComponent<IMovement>(); 
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
            var velocity = _rigidbody.velocity;
            while (velocity != Vector2.zero)
            {
                Vector2.Lerp(velocity, Vector2.zero, _transitionTime);
                yield return null;
            }
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
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
            else
                collision.gameObject.GetComponent<Lives>().LoseLife();
        }

        private IEnumerator Die()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.AddTorque(1, ForceMode2D.Impulse);
            _mainCollider.enabled = false;
            while (transform.position.y > -20)
                yield return null;
            DestroyImmediate(gameObject);
        }
        #endregion
    }
}