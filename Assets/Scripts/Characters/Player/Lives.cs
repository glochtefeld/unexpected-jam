using System;
using System.Collections;
using UnityEngine;

public class Lives : MonoBehaviour
{
    #region Serialized Fields
#pragma warning disable CS0649
    [SerializeField] private int _maxLives;
#pragma warning restore CS0649
    #endregion

    public int CurrentLives { set; get; }
    // Start is called before the first frame update
    void Start()
    {
        CurrentLives = _maxLives;
    }

    public void LoseLife() 
    { 
        Debug.Log("Lost life");
        if (CurrentLives > 0)
            CurrentLives--;
        else
        {
            StartCoroutine(Die());
            return;
        }
        StartCoroutine(Invulnerability());

    }

    private IEnumerator Invulnerability()
    {
        throw new NotImplementedException();
    }

    private IEnumerator Die()
    {
        throw new NotImplementedException();
    }
}
/* Number of lives that the player currently has. */