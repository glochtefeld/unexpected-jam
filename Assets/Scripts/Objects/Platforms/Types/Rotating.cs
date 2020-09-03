using UnityEngine;

namespace Unexpected.Objects.Platforms.Types
{
    public class Rotating : MonoBehaviour, IPlatform
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [Range(-1, 1)]
        [SerializeField] private int _direction;
        [SerializeField] private int _degreesPerSecond;
    #pragma warning restore CS0649
        #endregion

        #region Monobehaviour
        void Start()
        {
        
        }

        void Update()
        {
        
        }
        #endregion

        public void Activate() =>
            transform.Rotate(
                0,
                0,
                _degreesPerSecond * Time.fixedDeltaTime * _direction);
    }
}
