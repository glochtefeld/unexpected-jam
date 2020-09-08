using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    #region Serialized Fields
#pragma warning disable CS0649
    [SerializeField] private float _maxTime;
    [SerializeField] private TMPro.TMP_Text _timerText;
#pragma warning restore CS0649
    #endregion

    private float _remainingTime;
    private Lives _player;

    #region Monobehaviour
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<Lives>();
        _remainingTime = _maxTime;
    }
    private void Update()
    {
        if (PauseTime.Paused)
            return;
        if (_remainingTime < 0.01f)
        {
            _player.Die();
            return;
        }
        _remainingTime -= Time.deltaTime;
        _timerText.text = $"Time remaining: {Mathf.Round(_remainingTime)}s";

    }
    #endregion
}
