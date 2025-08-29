using System.Timers;

namespace FitnessTimer;

public partial class MainPage : ContentPage
{
    private System.Timers.Timer _timer;
    private int _totalSeconds;
    private int _remainingSeconds;
    private bool _isRunning = false;

    public MainPage()
    {
        InitializeComponent();

        // Timer setup
        _timer = new System.Timers.Timer(1000); // 1 second interval
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        if (!_isRunning)
        {
            // Get time from input fields
            if (int.TryParse(MinutesEntry.Text, out int minutes) &&
                int.TryParse(SecondsEntry.Text, out int seconds))
            {
                _totalSeconds = (minutes * 60) + seconds;
                _remainingSeconds = _totalSeconds;

                if (_remainingSeconds > 0)
                {
                    _isRunning = true;
                    _timer.Start();

                    StartButton.Text = "Pause";
                    UpdateDisplay();
                }
            }
        }
        else
        {
            // Pause functionality
            _isRunning = false;
            _timer.Stop();
            StartButton.Text = "Start";
        }
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        _isRunning = false;
        _timer.Stop();
        _remainingSeconds = 0;
        StartButton.Text = "Start";
        UpdateDisplay();
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        _isRunning = false;
        _timer.Stop();
        _remainingSeconds = _totalSeconds;
        StartButton.Text = "Start";
        UpdateDisplay();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _remainingSeconds--;

        // Update UI on main thread
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateDisplay();

            // Timer finished
            if (_remainingSeconds <= 0)
            {
                _isRunning = false;
                _timer.Stop();
                StartButton.Text = "Start";
                DisplayAlert("Zeit ist um!", "Dein Training ist beendet!", "OK");
            }
        });
    }

    private void UpdateDisplay()
    {
        int minutes = _remainingSeconds / 60;
        int seconds = _remainingSeconds % 60;
        TimerDisplay.Text = $"{minutes:D2}:{seconds:D2}";

        // Color change based on remaining time
        if (_remainingSeconds <= 10 && _remainingSeconds > 0)
        {
            TimerDisplay.TextColor = Colors.Red;
        }
        else if (_remainingSeconds == 0)
        {
            TimerDisplay.TextColor = Colors.Gray;
        }
        else
        {
            TimerDisplay.TextColor = Colors.Blue;
        }
    }
}
