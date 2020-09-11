using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private GameObject _player;
    #region Monobehaviour
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != _player)
            return;
        SceneManager.LoadScene(0);
        // TODO: Level cleanup as necessary, destroy audio sources/timers/etc
    }
    #endregion
}
