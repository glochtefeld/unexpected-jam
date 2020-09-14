using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

namespace Unexpected.UI
{
    public class ActionAssignment : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private CanvasSwitcher _switcher;
        [Header("Main Buttons")]
        [SerializeField] private Button _start;
        [SerializeField] private Button _toOptions;
        [SerializeField] private Button _toCredits;
        [SerializeField] private Button _quit;
        [Header("Back to Main buttons")]
        [SerializeField] private Button _fromOptionsToStart;
        [SerializeField] private Button _fromCreditsToStart;
#pragma warning restore CS0649
        #endregion

        #region Monobehaviour
        void Start()
        {
            _start.onClick.AddListener(() => SceneManager.LoadScene(1));
            _toOptions.onClick.AddListener(() => _switcher.SwitchCanvas(_switcher[1]));
            _toCredits.onClick.AddListener(() => _switcher.SwitchCanvas(_switcher[2]));
            _fromOptionsToStart.onClick.AddListener(() => _switcher.SwitchCanvas(_switcher[0]));
            _fromCreditsToStart.onClick.AddListener(() => _switcher.SwitchCanvas(_switcher[0]));
            _quit.onClick.AddListener(() => Application.Quit());

        }
        #endregion
    }
}