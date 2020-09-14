using System.Linq;
using UnityEngine;

namespace Unexpected.Objects.Platforms.Types
{
    public class BackAndForth : MonoBehaviour, IPlatform
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Transform[] _positions;
        [Range(0f,5f)]
        [SerializeField] private float _speed;
        [SerializeField] private float _smoothMovement;
#pragma warning restore CS0649
        #endregion
        private int _target = 0;
        private float _progress = 0f;
        private Vector2 _velocity = Vector2.zero;

        public void Activate()
        {
            if (Equal(transform.position, _positions[_target].position))
            {
                _target++;
                _target = (_target >= _positions.Length) ? 0 : _target;
            }

            transform.position = Vector2.SmoothDamp(
                transform.position,
                _positions[_target].position,
                ref _velocity,
                _smoothMovement);

            //transform.position = Vector2.Lerp(
            //    transform.position, 
            //    _positions[_target].position,
            //    _speed * Time.fixedDeltaTime);
            //_progress = Mathf.Lerp(_progress, 1f, _speed * Time.fixedDeltaTime);
        }

        private bool Equal(Vector2 a, Vector2 b)
        {
            if (Mathf.Floor(a.x) == Mathf.Floor(b.x)
                && Mathf.Floor(a.y) == Mathf.Floor(b.y))
                return true;
            return false;
        }

    }
}

/* Lerps a platform in a circuit between n positions. For a smoother
 * movement, Vector2.SmoothDamp() could be used. */