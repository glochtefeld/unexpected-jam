using UnityEngine;
using UnityEngine.InputSystem;

namespace Unexpected.Player
{
    public class Movement2D : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Controller2D _controller;
        [SerializeField] private float _runSpeed = 40f;
#pragma warning restore CS0649
        #endregion

        private float _horizontalMove = 0f;
        private bool _jumping;
        private bool _crouching;

        private void FixedUpdate()
        {
            _controller.Move(
                _horizontalMove * Time.fixedDeltaTime, 
                _crouching, 
                _jumping);
            _jumping = false;
        }

        public void Move(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();
            _horizontalMove = input.x * _runSpeed;
            if (input.y > 0)
                _jumping = true;
            if (input.y < 0)
                _crouching = true;
            else
                _crouching = false;
        }
    }
}
/* This uses the new InputSystem package for unity, which is more 
   efficient because it relies on events being fired off instead of
   the Update() method. I think. */