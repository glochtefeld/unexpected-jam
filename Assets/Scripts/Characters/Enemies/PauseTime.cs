using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Unexpected.Enemy
{
    public class PauseTime : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Material _grayscaleMat;
        [SerializeField] private float _transitionTime = 0.5f;
        [SerializeField] private AudioMixer _mixer;
#pragma warning restore CS0649
        #endregion

        private bool _transitioning = false;
        private static bool _ignoreInput = false;
        public static bool Paused { private set; get; }

        private void Start()
        {
            Paused = false;
            SetPitch(1);
            SetGrayscale(0);
        }

        public void Pause(UnityEngine.InputSystem.InputAction.CallbackContext c)
        {
            // The callback context will fire three times, we only want one coroutine
            if (_ignoreInput || _transitioning == false)
            {
                Paused = !Paused;
                _transitioning = true;
                StartCoroutine(GrayscaleRoutine(Paused));
            }
        }

        public void EndLevelPause()
        {
            _ignoreInput = true;
            Paused = true;
            StartCoroutine(GrayscaleRoutine(Paused));

        }

        public void UnPause()
        {
            Paused = false;
            SetPitch(1);
        }

        private IEnumerator GrayscaleRoutine(bool turnGray)
        {
            float time = 0f;
            while (time < _transitionTime)
            {
                float ratio = time / _transitionTime;
                float grayAmount = turnGray ? ratio : 1 - ratio;
                float pitch = turnGray ? 1 - ratio : ratio;
                SetGrayscale(grayAmount);
                SetPitch(pitch);
                time += Time.deltaTime;
                yield return null;
            }
            SetGrayscale(turnGray ? 1 : 0);
            _transitioning = false;
        }

        private void SetGrayscale(float amount) =>
            _grayscaleMat.SetFloat("_GrayscaleAmount", amount);

        private void SetPitch(float amount) =>
            _mixer.SetFloat("BGMPitch", amount);
    }
}

/* This script controls both the movement of all enemies (by
 * short circuiting their FixedUpdate() methods) and the color
 * of all sprites with the Grayscale Material. */