using UnityEngine;
using UnityEngine.InputSystem;

namespace Unexpected.Player
{
    public class Movement2D : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Controller2D _controller;
        [SerializeField] private Animator _animator;
        [Header("AUDIO")]
        [SerializeField] private AudioSource _sfxPlayer;
        [SerializeField] private AudioClip _jumpSFX;
        [SerializeField] private AudioClip _walkSFX;
        [SerializeField] private float _footstepRapidity;
#pragma warning restore CS0649
        #endregion

        private float _horizontalMove = 0f;
        private bool _jumping;
        private bool _crouching;
        private bool _ignoreInput = false;
        private float _timeSinceLastFootstep = 0f;
        private Vector2 _lastInput = Vector2.zero;

        private void FixedUpdate()
        {
            if (_lastInput.y > 0)
                _animator.SetBool("isJumping", true);

            _timeSinceLastFootstep += Time.fixedDeltaTime;
            if (_timeSinceLastFootstep > _footstepRapidity
                && _jumping == false
                && _horizontalMove != 0f)
            {
                _sfxPlayer.PlayOneShot(_walkSFX);
                _timeSinceLastFootstep = 0f;
            }
            _controller.Move(
                _horizontalMove * Time.fixedDeltaTime, 
                _crouching, 
                _jumping);
            _jumping = false;
            
        }

        public void OnLanding()
        {
            _animator.SetBool("isJumping", false);
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (_ignoreInput)
                return;
            var input = context.ReadValue<Vector2>();
            _horizontalMove = input.x;
            _animator.SetFloat("speed", Mathf.Abs(_horizontalMove));
            Debug.Log(input.y);
            if (input.y > 0)
            {
                _jumping = true;
                _sfxPlayer.PlayOneShot(_jumpSFX);
                _animator.SetBool("isJumping", true);
            }
            if (input.y < 0)
                _crouching = true;
            else
                _crouching = false;
            _lastInput = input;
        }

        public void IgnoreInput() => _ignoreInput = true;

    }
}
/* Movement 2D processes movement input from the keyboard and
 * any attached controllers.
 * 
 * This uses the new InputSystem package for unity, which is more 
   efficient because it relies on events being fired off instead of
   the Update() method. I think. */