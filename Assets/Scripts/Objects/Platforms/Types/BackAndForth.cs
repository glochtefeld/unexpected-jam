using UnityEngine;

namespace Unexpected.Objects.Platforms.Types
{
    public class BackAndForth : MonoBehaviour, IPlatform
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Transform[] _positions;
        [Range(0f,5f)]
        [SerializeField] private int _speed;
#pragma warning restore CS0649
        #endregion
        private int _target = 0;
        private float _progress = 0f;

        public void Activate()
        {
            if (_progress > 0.9f)
            {
                _progress = 0f;
                _target++;
                _target = (_target >= _positions.Length) ? 0 : _target;
            }

            transform.position = Vector2.Lerp(
                transform.position, 
                _positions[_target].position,
                _speed * Time.fixedDeltaTime);
            _progress = Mathf.Lerp(_progress, 1f, _speed * Time.fixedDeltaTime);
        }

    }
}

/* Lerps a platform in a circuit between n positions. For a smoother
 * movement, Vector2.SmoothDamp() could be used. */