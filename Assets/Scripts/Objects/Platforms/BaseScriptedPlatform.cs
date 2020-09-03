using Unexpected.Objects.Platforms.Types;
using UnityEngine;

namespace Unexpected.Objects.Platforms
{
    [RequireComponent(typeof(IPlatform))]
    public class BaseScriptedPlatform : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649

#pragma warning restore CS0649
        #endregion

        private IPlatform _platformType;

        #region Monobehaviour
        private void Awake()
        {
            _platformType = GetComponent<IPlatform>();
        }

        private void FixedUpdate()
        {
            if (!PauseTime.Paused)
                _platformType.Activate();
        }
        #endregion

    }
}
