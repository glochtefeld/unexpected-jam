using Unexpected.Enemy.Movement;
using UnityEngine;

namespace Unexpected.Enemy
{
    [RequireComponent(typeof(IMovement))]
    public class BaseEnemy : MonoBehaviour
    {
        public int health;
        public IMovement movement;

        private void Awake()
        {
            movement = gameObject.GetComponent<IMovement>();
        }


        void FixedUpdate()
        {
            movement.Move();
        }
    }
}