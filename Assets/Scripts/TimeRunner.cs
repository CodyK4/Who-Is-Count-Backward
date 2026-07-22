using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeRunner : MonoBehaviour
{
    //this script is responsible for running the in-game time, triggering events when the time expires, and updating the UI with the current time.

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

    [Header("Events")]
    [SerializeField] private UnityEvent onTimeExpired;

    private float elapsedRealSeconds;
    private bool isRunning;
    private bool hasFinished;

    public float Progress { get; private set; }
    public float RemainingRealSeconds { get; private set; }

    //lambdas to calculate the time in seconds for the game duration, real duration, and start time.
    private float RealDurationSeconds => realGameLengthMinutes * 60f;
    private float GameDurationSeconds => (timeLengthHours * 60f + additionalGameMinutes) * 60f;
    private float StartTimeSeconds => (startHour * 60f + startMinute) * 60f;

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (!isRunning || hasFinished)
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
}

