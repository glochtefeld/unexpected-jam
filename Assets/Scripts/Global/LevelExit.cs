using UnityEngine;
using UnityEngine.SceneManagement;
using Unexpected.UI;
using System.Collections;
using Unexpected.Player;
using TMPro;
using Unexpected.Enemy;

namespace Unexpected
{
    public class LevelExit : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Timer _timer;
        [SerializeField] private GameObject _pointTallyCanvas;
        [SerializeField] private TMP_Text _timerValue;
        [SerializeField] private TMP_Text _pointValue;
#pragma warning restore CS0649
        #endregion
        private GameObject _player;
        private GameObject _playerSFX;
        #region Monobehaviour
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerSFX = _player.transform.Find("SFX Player").gameObject;
            _playerSFX.SetActive(true);
            _pointTallyCanvas.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != _player)
                return;
            
            _timer.StopTimer();
            GameObject.FindGameObjectWithTag("Player")
                .GetComponent<Movement2D>().IgnoreInput();
            _player.GetComponent<Rigidbody2D>().constraints = 
                RigidbodyConstraints2D.FreezeAll;
            GameObject.FindGameObjectWithTag("TimeScale")
                .GetComponent<PauseTime>().EndLevelPause();
            _playerSFX.SetActive(false);

            // Activate End-of-level panel
            _pointTallyCanvas.SetActive(true);

            Destroy(GameObject.Find("CheckpointController"));

            // Tally up points
            StartCoroutine(TallyPoints((int)_timer.CurrentTime));

        }
        #endregion

        public void LoadNextLevel(int idx) => SceneManager.LoadScene(idx);

        private IEnumerator TallyPoints(int timeLeft)
        {
            int pointAcc = 0;
            for (int i = timeLeft - 1; i >= 0; i--)
            {
                _timerValue.text = $"Time Left: {timeLeft}";
                _pointValue.text = $"Points: {pointAcc}";
                timeLeft--;
                pointAcc++;
                yield return new WaitForSeconds(0.05f);
                yield return null;
            }
            _timerValue.text = $"Time Left: 0";
            _pointValue.text = $"Points: {pointAcc++}";

        }
    }
}