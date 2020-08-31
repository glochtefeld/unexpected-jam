using UnityEngine;
namespace Unexpected.Enemy.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BackAndForth : MonoBehaviour, IMovement
    {
        public Collider2D floorCollider;
        public Rigidbody2D rigidbody2d;
        public int speed;
        public LayerMask ground;
        [Range(0, 1)]
        public float movementSmoothing;

        private int _direction = 1;
        private Vector3 _velocity = Vector3.zero;

        public void Move() 
        {
            Vector3 targetVelocity = new Vector2(
                _direction * Time.fixedDeltaTime * speed,
                rigidbody2d.velocity.y);
            rigidbody2d.velocity = Vector3.SmoothDamp(
                rigidbody2d.velocity,
                targetVelocity,
                ref _velocity,
                movementSmoothing);
        }

        private void OnTriggerExit2D(Collider2D other) => Flip();

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