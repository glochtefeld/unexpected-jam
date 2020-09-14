using UnityEngine;

namespace Unexpected
{
    public class CheckpointController : MonoBehaviour
    {
        public static CheckpointController Instance { private set; get; }

        public Vector3 StartPosition { set; get; }

        void Awake()
        {
            if (Instance == null)
            {
                StartPosition = gameObject.transform.position;
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else
                Destroy(gameObject);
        }
    }
}