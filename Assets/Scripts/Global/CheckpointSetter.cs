using UnityEngine;
namespace Unexpected
{
    public class CheckpointSetter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _trigger;

        private GameObject _player;

        private void Start()
        {
            _player = GameObject.Find("Player");
            _trigger.gameObject.SetActive(true);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject != _player)
                return;
            //Debug.Log($"Collided with {other.transform.name}");
            transform.parent.GetComponent<CheckpointController>()
                .StartPosition = gameObject.transform.position;
            _trigger.Play();
            Debug.Log($"Checkpoint set to {gameObject.transform.position}");
            _trigger.gameObject.SetActive(false);
        }
    }
}