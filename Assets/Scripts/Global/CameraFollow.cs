using System;
using UnityEngine;

namespace Unexpected
{
    public class CameraFollow : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
#pragma warning disable IDE0044
        [Range(0, 1f)]
        [SerializeField] private float _movementSmoothing;
        [SerializeField] private bool _verticalScrolling;
        [SerializeField] private int _levelBoundLeft;
        [SerializeField] private int _levelBoundRight;
        [SerializeField] private int _levelBoundTop;
        [SerializeField] private int _levelBoundBottom;
#pragma warning restore IDE0044
#pragma warning restore CS0649
        #endregion

        private GameObject _player;
        private Vector3 _velocity = Vector3.zero;

        #region Monobehaviour
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void FixedUpdate()
        {
            var targetPosition = _player.transform.position;
            targetPosition.z = -10f;
            if (_verticalScrolling)
                targetPosition.x = transform.position.x;
            targetPosition.x = Mathf.Clamp(
                targetPosition.x,
                _levelBoundLeft,
                _levelBoundRight);
            targetPosition.y = Mathf.Clamp(
                targetPosition.y,
                _levelBoundBottom,
                _levelBoundTop);
            transform.position =
                Vector3.SmoothDamp(
                    transform.position,
                    targetPosition,
                    ref _velocity,
                    _movementSmoothing);
        }
        #endregion
    }
}
/* Finds the player in the scene and follows them around smoothly. */
