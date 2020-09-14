using System.Collections.Generic;
using UnityEngine;

namespace Unexpected.UI
{
    public class CanvasSwitcher : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649

#pragma warning restore CS0649
        #endregion

        private List<Canvas> _canvasOptions = new List<Canvas>();
        private Canvas _activeCanvas;
        #region Monobehaviour
        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _canvasOptions.Add(transform.GetChild(i).GetComponent<Canvas>());
            }
            _activeCanvas = _canvasOptions[0];
        }
        #endregion

        public void SwitchCanvas(Canvas to)
        {
            _activeCanvas.gameObject.SetActive(false);
            to.gameObject.SetActive(true);
            _activeCanvas = to;
        }

        public Canvas this[int i]
        {
            get { return _canvasOptions[i]; }
        }
    }
}