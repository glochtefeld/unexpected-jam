using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Unexpected.Enemy;

namespace Unexpected.UI
{
    public class Timer : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private float _timeLimit;
        [SerializeField] private TMP_Text _timer;
        [Header("Game Over Canvas")]
        [SerializeField] private CanvasGroup _gameOver;
        [SerializeField] private float _transitionTime;
        [SerializeField] private AudioSource _backgroundMusic;
#pragma warning restore CS0649
        #endregion

        private bool _levelOver = false;
        public float CurrentTime { set; get; }
        #region Monobehaviour
        void Start()
        {
            _gameOver.alpha = 0;
            _gameOver.gameObject.SetActive(false);

            CurrentTime = _timeLimit;
        }

        private void FixedUpdate()
        {
            if (PauseTime.Paused || _levelOver)
                return;
            if (CurrentTime < 1)
                EndGame();
            CurrentTime -= Time.fixedDeltaTime;
            _timer.text = $"Time Left: {Mathf.Floor(CurrentTime)}";
        }
        #endregion

        private void EndGame()
        {
            _gameOver.gameObject.SetActive(true);
            StartCoroutine(GameOverCanvasOpacity());
            GameObject.FindGameObjectWithTag("TimeScale")
                .GetComponent<PauseTime>().EndLevelPause();
        }

        private IEnumerator GameOverCanvasOpacity()
        {
            float time = 0f;
            while (time < _transitionTime)
            {
                _gameOver.alpha = time / _transitionTime;
                time += Time.deltaTime;
                yield return null;
            }
            _gameOver.alpha = 1;
            _backgroundMusic.Stop();
        }

        public void StopTimer() => _levelOver = true;
    }
}