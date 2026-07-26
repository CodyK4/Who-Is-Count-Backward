using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TimeRunner : MonoBehaviour
{
    // This script is responsible for running the in-game time, triggering events when the time expires, and updating the UI with the current time.
    // Based on an  older script from a previous project, and has been modified to fit the needs of this project.

        [Header("Start Time")]
    [Range(0, 23)] public int startHour = 21;
    [Range(0, 59)] public int startMinute = 0;

        [Header("In-Game Time Length")]
    [Range(1, 24)] public int timeLengthHours = 3;

    [Range(0, 59)]
    [SerializeField] private int additionalGameMinutes;

        [Header("Real Game Length")]
    [Min(1f)]
    [Tooltip("How Many real-world minutes the game lasts.")]
    [SerializeField] private float realGameLengthMinutes = 10f;

        [Header("UI Output")]
    [SerializeField] private TMP_Text timeText;

    public event System.Action onTimeExpired;

    private float elapsedRealSeconds;
    private bool isRunning;
    private bool hasFinished;
    private bool timePaused;

    public float Progress { get; private set; }
    public float RemainingRealSeconds { get; private set; }

    // Lambdas to calculate the time in seconds for the game duration, real duration, and start time.
    private float RealDurationSeconds => realGameLengthMinutes * 60f;
    private float GameDurationSeconds => (timeLengthHours * 60f + additionalGameMinutes) * 60f;
    private float StartTimeSeconds => (startHour * 60f + startMinute) * 60f;

    public event Action<int, int> OnMinuteChanged;

    public int CurrentHour { get; private set; }
    public int CurrentMinute { get; private set; }

    private int previousMinute = -1;

    public string CurrentTimeString => $"{CurrentHour:00}:{CurrentMinute:00}";

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!isRunning || hasFinished || timePaused)
        {
            return;
        }

        elapsedRealSeconds += Time.deltaTime;

        Progress = Mathf.Clamp01(elapsedRealSeconds / RealDurationSeconds);
        RemainingRealSeconds = Mathf.Max(0f, RealDurationSeconds - elapsedRealSeconds);

        UpdateTimeUI();

        if (Progress >= 1f)
            FinishTimer();
    }

    public void StartTimer()
    {
        // Starts the timer, resets elapsed time & updates UI.
        elapsedRealSeconds = 0f;
        RemainingRealSeconds = RealDurationSeconds;

        Progress = 0f;
        isRunning = true;
        hasFinished = false;
        timePaused = false;

        UpdateTimeUI();
    }

    public void PauseTimer()
    {
        // Used for pausing the game
        isRunning = false;
    }

    public void ResumeTimer()
    {
        // Used for unpausing the game

        if (hasFinished == false)
        {
            isRunning = true;
        }
    }

    public void RestartTimer()
    {
        // Used for restarting the game
        StartTimer();
    }

    private void FinishTimer()
    {
        Progress = 1f;
        RemainingRealSeconds = 0f;

        isRunning = false;
        hasFinished = true;

        UpdateTimeUI();

        onTimeExpired?.Invoke();
    }

    public void SkipToMidnight()
    {
        timePaused = true;

        CurrentHour = 0;
        CurrentMinute = 0;

        UpdateTimeUI();
    }

    private void UpdateTimeUI()
    {
        float currentTimeSeconds = StartTimeSeconds + GameDurationSeconds * Progress;

        // Wraps the clock after 24
        currentTimeSeconds %= 24 * 60f * 60f;

        int hours = (int)(currentTimeSeconds / 3600f) % 24;
        int minutes = (int)(currentTimeSeconds / 60f) % 60;
        int seconds = (int)(currentTimeSeconds % 60f);

        CurrentHour = hours;
        CurrentMinute = minutes;

        if (CurrentMinute != previousMinute)
        // Trigger the OnMinuteChanged event when the minute changes, for backend events
        {
            previousMinute = CurrentMinute;
            Debug.Log($"Minute changed to {CurrentHour:00}:{CurrentMinute:00}");
            OnMinuteChanged?.Invoke(hours, minutes);
        }

        if (timeText == null)
        {
            return;
        }
        else
        {
            timeText.text = $"{hours:00}:{minutes:00}";
        }
    }
}

