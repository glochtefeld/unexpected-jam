using Unexpected.Enemy.Movement;
using UnityEngine;

namespace Unexpected.Enemy
{
    [RequireComponent(typeof(IMovement))]
    public class BaseEnemy : MonoBehaviour
    {
        public int health;
        private IMovement _movement;

        void Awake()
        {
            _movement = GetComponent<IMovement>();
        }

        void FixedUpdate()
        {
            _movement.Move();
        }
    }
}