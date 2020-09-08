using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Serialized Fields
#pragma warning disable CS0649
    [Range(0, 1f)]
    [SerializeField] private float _movementSmoothing;
    [SerializeField] private bool _verticalScrolling;
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
        transform.position =
            Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref _velocity,
                _movementSmoothing);
    }
    #endregion
}
/* Finds the player in the scene and follows them around smoothly. */
