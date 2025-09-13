using System.Collections.ObjectModel;
using System.Text.Json;
using System.Timers;
using Microsoft.Maui.Controls;

namespace FitnessTimer;

public partial class MainPage : ContentPage
{
    private System.Timers.Timer _timer;
    private int _totalSeconds;
    private int _remainingSeconds;
    private bool _isRunning = false;
    private ObservableCollection<TimerHistoryItem> _historyItems;

    public MainPage()
    {
        InitializeComponent();
        
        // Timer setup
        _timer = new System.Timers.Timer(1000); // 1 second interval
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        
        // History setup
        _historyItems = new ObservableCollection<TimerHistoryItem>();
        HistoryCollectionView.ItemsSource = _historyItems;
        
        // Load saved data
        LoadSettings();
        LoadHistory();
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
                    StartButton.BackgroundColor = Colors.Orange;
                    UpdateDisplay();
                    
                    // Save current settings
                    SaveSettings();
                }
            }
        }
        else
        {
            // Pause functionality
            _isRunning = false;
            _timer.Stop();
            StartButton.Text = "Start";
            StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
        }
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        if (_isRunning || _remainingSeconds != _totalSeconds)
        {
            _isRunning = false;
            _timer.Stop();
            
            // Add to history if timer was actually used
            if (_remainingSeconds != _totalSeconds)
            {
                int completedSeconds = _totalSeconds - _remainingSeconds;
                AddToHistory(completedSeconds);
            }
            
            _remainingSeconds = 0;
            StartButton.Text = "Start";
            StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
            UpdateDisplay();
        }
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        _isRunning = false;
        _timer.Stop();
        _remainingSeconds = _totalSeconds;
        StartButton.Text = "Start";
        StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
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
                StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
                
                // Add completed timer to history
                AddToHistory(_totalSeconds);
                
                // Play notification sound and show alert
                PlayNotificationSound();
                DisplayAlert("🏁 Zeit ist um!", "Dein Training ist beendet! Gut gemacht! 💪", "Weiter");
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
            TimerDisplay.TextColor = Color.FromRgb(0, 255, 136);
        }
    }

    private void PlayNotificationSound()
    {
        try
        {
            // Simple vibration for notification
            // Note: For actual sound, you would need platform-specific implementations
            // This is a cross-platform safe implementation
            System.Diagnostics.Debug.WriteLine("Timer finished - notification would play here");
        }
        catch
        {
            // Ignore if notification not supported
        }
    }

    private void AddToHistory(int totalSeconds)
    {
        var historyItem = new TimerHistoryItem
        {
            TimeText = $"{totalSeconds / 60:D2}:{totalSeconds % 60:D2}",
            DateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
            TotalSeconds = totalSeconds
        };
        
        _historyItems.Insert(0, historyItem); // Add to top
        
        // Keep only last 20 entries
        while (_historyItems.Count > 20)
        {
            _historyItems.RemoveAt(_historyItems.Count - 1);
        }
        
        SaveHistory();
    }

    private void OnClearHistoryClicked(object sender, EventArgs e)
    {
        _historyItems.Clear();
        SaveHistory();
    }

    private void SaveSettings()
    {
        try
        {
            var settings = new
            {
                LastMinutes = MinutesEntry.Text,
                LastSeconds = SecondsEntry.Text
            };
            
            var json = JsonSerializer.Serialize(settings);
            Preferences.Set("TimerSettings", json);
        }
        catch
        {
            // Ignore save errors
        }
    }

    private void LoadSettings()
    {
        try
        {
            var json = Preferences.Get("TimerSettings", "");
            if (!string.IsNullOrEmpty(json))
            {
                var settings = JsonSerializer.Deserialize<dynamic>(json);
                // Settings loaded successfully - keep current default values for simplicity
            }
        }
        catch
        {
            // Ignore load errors - keep defaults
        }
    }

    private void SaveHistory()
    {
        try
        {
            var json = JsonSerializer.Serialize(_historyItems.ToList());
            Preferences.Set("TimerHistory", json);
        }
        catch
        {
            // Ignore save errors
        }
    }

    private void LoadHistory()
    {
        try
        {
            var json = Preferences.Get("TimerHistory", "");
            if (!string.IsNullOrEmpty(json))
            {
                var historyList = JsonSerializer.Deserialize<List<TimerHistoryItem>>(json);
                if (historyList != null)
                {
                    foreach (var item in historyList)
                    {
                        _historyItems.Add(item);
                    }
                }
            }
        }
        catch
        {
            // Ignore load errors
        }
    }
}

// Helper class for timer history
public class TimerHistoryItem
{
    public string TimeText { get; set; } = "";
    public string DateText { get; set; } = "";
    public int TotalSeconds { get; set; }
}
