using System.Collections;
using Unexpected.Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unexpected.UI;

namespace Unexpected.Player
{
    public class Lives : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private int _maxLives;
        [SerializeField] private float _invulnTime;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Material _grayscale;
        [Header("Dead Canvas")]
        [SerializeField] private CanvasGroup _gameOverCanvas;
        [SerializeField] private float _transitionTime;
        [Header("AUDIO")]
        [SerializeField] private AudioSource _sfxPlayer;
        [SerializeField] private AudioClip _enemyHurt;
        [SerializeField] private AudioClip _playerHurt;
        [SerializeField] private AudioClip _playerDeath;
#pragma warning restore CS0649
        #endregion

        private int _currentLives;
        private bool _invulnerable = false;
        private bool _isAlreadyDead = false;
        private Health _healthContainer;

        #region Monobehaviour
        void Start()
        {
            _currentLives = _maxLives;
            _healthContainer = GameObject
                .FindGameObjectWithTag("HealthContainer")
                .GetComponent<Health>();
        }
        private void Update()
        {
            if (transform.position.y < -10)
                Die();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // TODO: Add checks for pickups, regex on name?
            var enemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (enemy == null)
                return;
            if (collision.transform.position.y
                < transform.GetChild(2).position.y)
            {
                StartCoroutine(enemy.Die());
                _sfxPlayer.PlayOneShot(_enemyHurt);
            }
            else if (!PauseTime.Paused)
                LoseLife();
        }
        #endregion

        public void LoseLife()
        {
            if (_invulnerable)
                return;
            //Debug.Log("Lost life");
            _currentLives--;
            if (_currentLives < 1)
            {
                Die();
                return;
            }
            _sfxPlayer.PlayOneShot(_playerHurt);
            StartCoroutine(Invulnerability());
            _healthContainer.SetHealth(_currentLives);
        }

        private IEnumerator Invulnerability()
        {
            // TODO: Make animation for being hit blink
            //Debug.Log("Invuln");
            _invulnerable = true;
            yield return new WaitForSeconds(_invulnTime);
            _invulnerable = false;
        }

        public void Die()
        {
            if (!_isAlreadyDead)
                StartCoroutine(DeathCoroutine());
            _sfxPlayer.PlayOneShot(_playerDeath);
            GameObject.FindGameObjectWithTag("TimeScale")
                .GetComponent<PauseTime>().EndLevelPause();
            _isAlreadyDead = true;
            _healthContainer.SetHealth(0);
            _grayscale.SetFloat("_GrayscaleAmount", 0);
        }

        private IEnumerator DeathCoroutine()
        {
            Debug.Log("Player is Dead");
            _gameOverCanvas.gameObject.SetActive(true);
            float time = 0;
            while (time < _transitionTime)
            {
                var ratio = time / _transitionTime;
                _gameOverCanvas.alpha = ratio;
                time += Time.deltaTime;
                yield return null;
            }
            _gameOverCanvas.alpha = 1;
        }
    }
}
/* Number of lives that the player currently has. */