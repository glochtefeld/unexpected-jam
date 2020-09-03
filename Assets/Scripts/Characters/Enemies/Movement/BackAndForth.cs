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
#pragma warning restore CS0649
        #endregion

        private int _direction = 1;
        private Vector3 _velocity = Vector3.zero;

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
        }

        public void Die() { }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == _ground.value)
                Flip();
        }

        private void Flip()
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            _direction *= -1;
        }
    }
}
/* The enemy will move forward and backward along a set platform,
 * stopping at the edge and reversing direction. */