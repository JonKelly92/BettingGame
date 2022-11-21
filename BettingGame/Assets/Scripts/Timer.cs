using UnityEngine;

/// <summary>
/// This class just counts down one second at a time from a given number and tells the UI to display it
/// </summary>
public class Timer : MonoBehaviour
{
    private float _timeRemaining;
    private bool _stopTimer;
    private string _formattedTime;

    void Awake()
    {
        _timeRemaining = 0;
        _stopTimer = true;
    }

    public void StartTimer(int startTime)
    {
        _timeRemaining = startTime;
        _stopTimer = false;
    }

    void Update()
    {
        if (_stopTimer)
            return;

        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0)
        {
            _formattedTime = "0";
            _stopTimer = true;
            EventManager.TimerEnded();
        }
        else
            _formattedTime = _timeRemaining.ToString("0");

        UIManager.Instance.UpdateTimer(_formattedTime);
    }
}