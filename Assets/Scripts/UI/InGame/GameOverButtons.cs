using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unexpected.Enemy;

namespace Unexpected.UI
{
    public class GameOverButtons : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Button _restart;
        [SerializeField] private Button _main;
        [SerializeField] private Button _quit;
#pragma warning restore CS0649
        #endregion

        private PauseTime _pause;
        #region Monobehaviour
        void Start()
        {
            _pause = GameObject.FindGameObjectWithTag("TimeScale")
                .GetComponent<PauseTime>();
            _restart.onClick.AddListener(() =>
            {
                _pause.UnPause();
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex);
            });
            _main.onClick.AddListener(() =>
            {
                _pause.UnPause();
                SceneManager.LoadScene(0);
            });
            _quit.onClick.AddListener(() => Application.Quit());
        }
        #endregion

    }
}