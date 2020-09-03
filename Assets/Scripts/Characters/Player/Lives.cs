using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    #region Serialized Fields
#pragma warning disable CS0649
    [SerializeField] private int _maxLives;
#pragma warning restore CS0649
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseLife() { Debug.Log("Lost life"); } // TODO: Implement
}
