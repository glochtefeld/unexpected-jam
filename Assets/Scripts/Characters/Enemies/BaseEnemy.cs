using System.Collections;
using Unexpected.Enemy.Movement;
using UnityEngine;

namespace Unexpected.Enemy
{
    [RequireComponent(typeof(IMovement))]
    public class BaseEnemy : MonoBehaviour
    {
        public int health;
        private IMovement _movement;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private bool _isPaused;
        private float _transitionTime = 0.5f;
        

        private void Awake()
        {
            _movement = gameObject.GetComponent<IMovement>();
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }


        void FixedUpdate()
        {
            if (!PauseTime.Paused && !_isPaused)
                _movement.Move();
            else if (!PauseTime.Paused && _isPaused)
            {
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                StartCoroutine(GrayscaleRoutine(false));
                _isPaused = false;
            }
            else if (PauseTime.Paused && !_isPaused)
            {
                StartCoroutine(GrayscaleRoutine(true));
                StartCoroutine(FreezePosition());
            }
        }

        #region Time Stop
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

        private IEnumerator GrayscaleRoutine(bool turnGray)
        {
            float time = 0f;
            while (time < _transitionTime)
            {
                float ratio = time / _transitionTime;
                float grayAmount = turnGray ? ratio : 1 - ratio;
                SetGrayscale(grayAmount);
                time += Time.deltaTime;
                yield return null;
            }
            SetGrayscale(turnGray ? 1 : 0);
            yield return null;
        }

        private void SetGrayscale(float amount) => 
            _spriteRenderer.material.SetFloat("_GrayscaleAmount", amount);

        #endregion

        #region Health and Damage
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collided");
            if (collision.gameObject.GetComponent<Lives>() == null)
                return;
            // Foot position
            if (collision.transform.GetChild(2).position.y > transform.position.y)
                StartCoroutine(Die());
            else
                collision.gameObject.GetComponent<Lives>().LoseLife();
        }

        private IEnumerator Die()
        {
            //Debug.Log("Enemy dead");
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.AddTorque(8, ForceMode2D.Impulse);
            while (transform.position.y > -20)
                yield return null;
            DestroyImmediate(gameObject);
        }
        #endregion
    }
}