using Unexpected.Objects.Platforms.Types;
using UnityEngine;
using Unexpected.Enemy;

namespace Unexpected.Objects.Platforms
{
    [RequireComponent(typeof(IPlatform))]
    public class BaseScriptedPlatform : MonoBehaviour
    {
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
/* This is a wrapper class that goes on all Dynamic platforms.
 * It simply calls the IMovement derived script attached (_platformType)
 * to do whatever it does, as long as time isn't paused. */