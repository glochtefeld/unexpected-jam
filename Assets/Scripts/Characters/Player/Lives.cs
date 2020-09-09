using System.Collections;
using Unexpected.Enemy;
using UnityEngine;

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

    public int CurrentLives { set; get; }
    private bool _invulnerable = false;
    private Color _invulnColor = new Color(255, 0, 0, 128);
    private bool _isAlreadyDead = false;


    #region Monobehaviour
    void Start()
    {
        CurrentLives = _maxLives;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        //if (_invulnerable)
        //    return;
        //Debug.Log("Lost life");
        //if (CurrentLives > 0)
        //    CurrentLives--;
        //else
        //{
        //    StartCoroutine(DeathCoroutine());
        //    return;
        //}
        //StartCoroutine(Invulnerability());

    }

    private IEnumerator Invulnerability()
    {
        _invulnerable = true;
        Color normal = _sprite.color;
        _sprite.color = _invulnColor;
        yield return new WaitForSeconds(_invulnTime);
        _sprite.color = normal;
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
        _deathFlag.SetActive(true);
        yield return null;
    }
}
/* Number of lives that the player currently has. */