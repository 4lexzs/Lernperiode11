using System.Collections.ObjectModel;
using System.Text.Json;
using System.Timers;
using Microsoft.Maui.Controls;

namespace FitnessTimer;

public partial class MainPage : ContentPage
{
    private System.Timers.Timer? _timer;
    private int _totalSeconds;
    private int _remainingSeconds;
    private bool _isRunning = false;
    private ObservableCollection<TimerHistoryItem> _historyItems = new();
    
    // Workout-spezifische Variablen
    private WorkoutPreset? _currentWorkout;
    private bool _isWorkoutMode = false;
    private bool _isRestPhase = false;
    private int _currentRound = 0;
    private int _totalRounds = 1;

    public MainPage()
    {
        InitializeComponent();
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        // Timer setup
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        
        // History setup  
        HistoryCollectionView.ItemsSource = _historyItems;
        
        // Workout presets laden
        LoadWorkoutPresets();
        
        // Load saved data
        LoadSettings();
        LoadHistory();
    }

    private void LoadWorkoutPresets()
    {
        var workouts = WorkoutPresets.LoadWorkouts();
        WorkoutPicker.ItemsSource = workouts;
        WorkoutPicker.ItemDisplayBinding = new Binding("Name");
    }

    private void OnWorkoutSelected(object sender, EventArgs e)
    {
        if (WorkoutPicker.SelectedItem is WorkoutPreset selectedWorkout)
        {
            _currentWorkout = selectedWorkout;
            
            // Update UI mit Workout-Daten
            MinutesEntry.Text = selectedWorkout.WorkoutMinutes.ToString();
            SecondsEntry.Text = selectedWorkout.WorkoutSeconds.ToString();
            
            WorkoutDescriptionLabel.Text = $"{selectedWorkout.Description} - {selectedWorkout.Rounds} Runden";
            
            _isWorkoutMode = true;
            _totalRounds = selectedWorkout.Rounds;
            _currentRound = 1;
            _isRestPhase = false;
            
            UpdateRoundDisplay();
            UpdateTimerModeDisplay();
        }
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        if (!_isRunning)
        {
            if (_isWorkoutMode && _currentWorkout != null)
            {
                StartWorkout();
            }
            else
            {
                StartManualTimer();
            }
        }
        else
        {
            // Pause functionality
            _isRunning = false;
            _timer?.Stop();
            StartButton.Text = "Start";
            StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
        }
    }

    private void StartWorkout()
    {
        if (_currentWorkout == null) return;

        if (!_isRestPhase)
        {
            // Training-Phase
            _totalSeconds = (_currentWorkout.WorkoutMinutes * 60) + _currentWorkout.WorkoutSeconds;
            TimerModeLabel.Text = $"Training - Runde {_currentRound}";
        }
        else
        {
            // Pause-Phase
            _totalSeconds = (_currentWorkout.RestMinutes * 60) + _currentWorkout.RestSeconds;
            TimerModeLabel.Text = $"Pause - Runde {_currentRound}";
        }

        _remainingSeconds = _totalSeconds;
        
        if (_remainingSeconds > 0)
        {
            _isRunning = true;
            _timer?.Start();
            StartButton.Text = "Pause";
            StartButton.BackgroundColor = Colors.Orange;
            UpdateDisplay();
            UpdateRoundDisplay();
        }
    }

    private void StartManualTimer()
    {
        if (int.TryParse(MinutesEntry.Text, out int minutes) && 
            int.TryParse(SecondsEntry.Text, out int seconds))
        {
            _totalSeconds = (minutes * 60) + seconds;
            _remainingSeconds = _totalSeconds;
            
            if (_remainingSeconds > 0)
            {
                _isRunning = true;
                _timer?.Start();
                StartButton.Text = "Pause";
                StartButton.BackgroundColor = Colors.Orange;
                TimerModeLabel.Text = "Training";
                UpdateDisplay();
                SaveSettings();
            }
            else
            {
                DisplayAlert("Ungültige Zeit", "Bitte geben Sie eine Zeit größer als 0 ein.", "OK");
            }
        }
        else
        {
            DisplayAlert("Eingabefehler", "Bitte geben Sie gültige Zahlen ein.", "OK");
        }
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        if (_isRunning || _remainingSeconds != _totalSeconds)
        {
            _isRunning = false;
            _timer?.Stop();
            
            // Add to history if timer was actually used
            if (_remainingSeconds != _totalSeconds && _totalSeconds > 0)
            {
                int completedSeconds = _totalSeconds - _remainingSeconds;
                string workoutName = _isWorkoutMode && _currentWorkout != null ? _currentWorkout.Name : "Manuell";
                AddToHistory(completedSeconds, workoutName);
            }
            
            // Reset workout state
            _isWorkoutMode = false;
            _currentWorkout = null;
            _currentRound = 0;
            _isRestPhase = false;
            WorkoutPicker.SelectedItem = null;
            
            _remainingSeconds = 0;
            StartButton.Text = "Start";
            StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
            TimerModeLabel.Text = "Training";
            RoundCounterLabel.Text = "";
            WorkoutDescriptionLabel.Text = "";
            UpdateDisplay();
        }
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        _isRunning = false;
        _timer?.Stop();
        _remainingSeconds = _totalSeconds;
        StartButton.Text = "Start";
        StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
        UpdateDisplay();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _remainingSeconds--;
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateDisplay();
            
            if (_remainingSeconds <= 0)
            {
                _timer?.Stop();
                _isRunning = false;
                StartButton.Text = "Start";
                StartButton.BackgroundColor = Color.FromRgb(0, 255, 136);
                
                if (_isWorkoutMode && _currentWorkout != null)
                {
                    HandleWorkoutPhaseComplete();
                }
                else
                {
                    // Normaler Timer beendet
                    AddToHistory(_totalSeconds, "Manuell");
                    PlayNotificationSound();
                    DisplayAlert("Zeit ist um!", "Dein Training ist beendet!", "Weiter");
                }
            }
        });
    }

    private void HandleWorkoutPhaseComplete()
    {
        if (_currentWorkout == null) return;

        if (!_isRestPhase)
        {
            // Training-Phase beendet, starte Pause-Phase
            _isRestPhase = true;
            
            if (_currentWorkout.RestMinutes > 0 || _currentWorkout.RestSeconds > 0)
            {
                PlayNotificationSound();
                DisplayAlert("Training beendet!", $"Runde {_currentRound} geschafft! Jetzt {_currentWorkout.RestMinutes}:{_currentWorkout.RestSeconds:D2} Pause.", "Pause starten")
                    .ContinueWith(task =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            StartWorkout(); // Starte Pause automatisch
                        });
                    });
            }
            else
            {
                // Keine Pause, direkt zur nächsten Runde
                _isRestPhase = false;
                _currentRound++;
                CheckWorkoutComplete();
            }
        }
        else
        {
            // Pause beendet, nächste Trainings-Runde oder Workout beendet
            _isRestPhase = false;
            _currentRound++;
            
            if (_currentRound <= _totalRounds)
            {
                PlayNotificationSound();
                DisplayAlert("Pause vorbei!", $"Starte Runde {_currentRound} von {_totalRounds}", "Los geht's!")
                    .ContinueWith(task =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            StartWorkout(); // Starte nächste Runde automatisch
                        });
                    });
            }
            else
            {
                CheckWorkoutComplete();
            }
        }
    }

    private void CheckWorkoutComplete()
    {
        if (_currentRound > _totalRounds)
        {
            // Workout komplett beendet
            string workoutName = _currentWorkout?.Name ?? "Workout";
            int totalWorkoutTime = (_totalRounds * ((_currentWorkout?.WorkoutMinutes ?? 0) * 60 + (_currentWorkout?.WorkoutSeconds ?? 0)));
            
            AddToHistory(totalWorkoutTime, $"{workoutName} ({_totalRounds} Runden)");
            
            PlayNotificationSound();
            DisplayAlert("Workout beendet!", $"Herzlichen Glückwunsch! Du hast das {workoutName}-Workout mit {_totalRounds} Runden geschafft!", "Toll gemacht!");
            
            // Reset
            OnStopClicked(this, EventArgs.Empty);
        }
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

    private void UpdateRoundDisplay()
    {
        if (_isWorkoutMode && _currentWorkout != null)
        {
            RoundCounterLabel.Text = $"Runde {_currentRound} von {_totalRounds}";
        }
        else
        {
            RoundCounterLabel.Text = "";
        }
    }

    private void UpdateTimerModeDisplay()
    {
        if (_isWorkoutMode && _currentWorkout != null)
        {
            if (!_isRestPhase)
            {
                TimerModeLabel.Text = $"Training - Runde {_currentRound}";
            }
            else
            {
                TimerModeLabel.Text = $"Pause - Runde {_currentRound}";
            }
        }
        else
        {
            TimerModeLabel.Text = "Training";
        }
    }

    private void PlayNotificationSound()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("Timer finished - notification sound would play here");
        }
        catch
        {
            // Ignore if notification not supported
        }
    }

    private void AddToHistory(int totalSeconds, string workoutName = "")
    {
        if (totalSeconds <= 0) return;
        
        var historyItem = new TimerHistoryItem
        {
            TimeText = $"{totalSeconds / 60:D2}:{totalSeconds % 60:D2}",
            DateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
            WorkoutName = workoutName,
            TotalSeconds = totalSeconds
        };
        
        _historyItems.Insert(0, historyItem);
        
        while (_historyItems.Count > 20)
        {
            _historyItems.RemoveAt(_historyItems.Count - 1);
        }
        
        SaveHistory();
    }

    private void OnClearHistoryClicked(object sender, EventArgs e)
    {
        if (_historyItems.Count > 0)
        {
            DisplayAlert("Historie löschen", "Möchten Sie wirklich alle Einträge löschen?", "Ja", "Nein")
                .ContinueWith(task =>
                {
                    if (task.Result)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            _historyItems.Clear();
                            SaveHistory();
                        });
                    }
                });
        }
    }

    private void SaveSettings()
    {
        try
        {
            var settings = new
            {
                LastMinutes = MinutesEntry.Text ?? "5",
                LastSeconds = SecondsEntry.Text ?? "0"
            };
            
            var json = JsonSerializer.Serialize(settings);
            Preferences.Set("TimerSettings", json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Speichern der Einstellungen: {ex.Message}");
        }
    }

    private void LoadSettings()
    {
        try
        {
            var json = Preferences.Get("TimerSettings", "");
            if (!string.IsNullOrEmpty(json))
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                
                if (root.TryGetProperty("LastMinutes", out var minutes))
                {
                    MinutesEntry.Text = minutes.GetString() ?? "5";
                }
                if (root.TryGetProperty("LastSeconds", out var seconds))
                {
                    SecondsEntry.Text = seconds.GetString() ?? "0";
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Laden der Einstellungen: {ex.Message}");
            MinutesEntry.Text = "5";
            SecondsEntry.Text = "0";
        }
    }

    private void SaveHistory()
    {
        try
        {
            var json = JsonSerializer.Serialize(_historyItems.ToList());
            Preferences.Set("TimerHistory", json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Speichern der Historie: {ex.Message}");
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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Laden der Historie: {ex.Message}");
        }
    }
}

public class TimerHistoryItem
{
    public string TimeText { get; set; } = "";
    public string DateText { get; set; } = "";
    public string WorkoutName { get; set; } = "";
    public int TotalSeconds { get; set; }
}
