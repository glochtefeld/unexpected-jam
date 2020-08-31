using UnityEngine;
namespace Unexpected.Enemy.Movement
{
    public class BackAndForth : MonoBehaviour, IMovement
    {

        private void FixedUpdate()
        {

        }

        public void Move() { }
    }
}
/* The enemy will move forward and backward along a set platform,
 * stopping at the edge and reversing direction. */