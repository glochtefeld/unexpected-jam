using System.Collections;
using Unexpected.Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Lives : MonoBehaviour
{
    #region Serialized Fields
#pragma warning disable CS0649
    [SerializeField] private int _maxLives;
    [SerializeField] private float _invulnTime;
    [SerializeField] private SpriteRenderer _sprite;
    // TODO: Add reference to UI
    [SerializeField] private GameObject _deathFlag;
#pragma warning restore CS0649
    #endregion

    private int _currentLives;
    private bool _invulnerable = false;
    private bool _isAlreadyDead = false;

    #region Monobehaviour
    void Start()
    {
        _currentLives = _maxLives;
    }
    private void Update()
    {
        if (transform.position.y < -10)
            Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: Add checks for pickups, regex on name?
        var enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (enemy == null)
            return;
        if (collision.transform.position.y
            < transform.GetChild(2).position.y)
            StartCoroutine(enemy.Die());
        else if (!PauseTime.Paused)
            LoseLife();
    }
    #endregion

    public void LoseLife() 
    {
        if (_invulnerable)
            return;
        Debug.Log("Lost life");
        if (_currentLives > 0)
            _currentLives--;
        else
        {
            Die();
            return;
        }
        StartCoroutine(Invulnerability());

    }

    private IEnumerator Invulnerability()
    {
        // TODO: Make animation for being hit blink
        _invulnerable = true;
        yield return new WaitForSeconds(_invulnTime);
        _invulnerable = false;
    }

    public void Die()
    {
        if (!_isAlreadyDead) 
            StartCoroutine(DeathCoroutine());
        _isAlreadyDead = true;
    }

    private IEnumerator DeathCoroutine()
    {
        Debug.Log("Player is Dead");
        //_deathFlag.SetActive(true);
        // loads current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null;
    }
}
/* Number of lives that the player currently has. */