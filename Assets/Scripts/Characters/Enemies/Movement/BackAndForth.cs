using System.Collections.Generic;
using UnityEngine;
namespace Unexpected.Enemy.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BackAndForth : MonoBehaviour, IMovement
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Collider2D _floorCollider;
        [SerializeField] private Rigidbody2D _rigidbody2d;
        [SerializeField] private int _speed;
        [SerializeField] private LayerMask _ground;
        [Range(0, 1)]
        [SerializeField] private float _movementSmoothing;
        [Range(-1,1)]
        [SerializeField] private int _direction;
#pragma warning restore CS0649
        #endregion

        private Vector3 _velocity = Vector3.zero;
        private HashSet<Collider2D> _groundColliders
            = new HashSet<Collider2D>();
        public void Move() 
        {
            Vector3 targetVelocity = new Vector2(
                _direction * Time.fixedDeltaTime * _speed,
                _rigidbody2d.velocity.y);
            _rigidbody2d.velocity = Vector3.SmoothDamp(
                _rigidbody2d.velocity,
                targetVelocity,
                ref _velocity,
                _movementSmoothing);

            transform.localScale = (_rigidbody2d.velocity.x <= 0f)
                ? new Vector3(-1f, 1f, 1f)
                : Vector3.one;
        }

        public void Die() { }

        private void OnTriggerExit2D(Collider2D other)
        {
            _groundColliders.Remove(other);
            if (_groundColliders.Count ==0)
                _direction *= -1;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _groundColliders.Add(other);
        }
    }
}
/* The enemy will move forward and backward along a set platform,
 * stopping at the edge and reversing direction. */