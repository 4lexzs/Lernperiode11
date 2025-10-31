using System.Text.Json;

namespace FitnessTimer;

public class WorkoutPreset
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int WorkoutMinutes { get; set; }
    public int WorkoutSeconds { get; set; }
    public int RestMinutes { get; set; }
    public int RestSeconds { get; set; }
    public int Rounds { get; set; } = 1;
    public string Category { get; set; } = "";
}

public static class WorkoutPresets
{
    public static List<WorkoutPreset> GetDefaultWorkouts()
    {
        return new List<WorkoutPreset>
        {
            new WorkoutPreset
            {
                Name = "Kurzes HIIT",
                Description = "Schnelles High-Intensity Training",
                WorkoutMinutes = 0,
                WorkoutSeconds = 30,
                RestMinutes = 0,
                RestSeconds = 15,
                Rounds = 8,
                Category = "HIIT"
            },
            new WorkoutPreset
            {
                Name = "Tabata Classic",
                Description = "4 Minuten Tabata Training",
                WorkoutMinutes = 0,
                WorkoutSeconds = 20,
                RestMinutes = 0,
                RestSeconds = 10,
                Rounds = 8,
                Category = "Tabata"
            },
            new WorkoutPreset
            {
                Name = "Kraft Training",
                Description = "Krafttraining mit längeren Pausen",
                WorkoutMinutes = 1,
                WorkoutSeconds = 0,
                RestMinutes = 2,
                RestSeconds = 0,
                Rounds = 5,
                Category = "Kraft"
            },
            new WorkoutPreset
            {
                Name = "Cardio Intervall",
                Description = "Moderate Cardio-Intervalle",
                WorkoutMinutes = 2,
                WorkoutSeconds = 0,
                RestMinutes = 1,
                RestSeconds = 0,
                Rounds = 6,
                Category = "Cardio"
            },
            new WorkoutPreset
            {
                Name = "Stretching",
                Description = "Dehnungsübungen",
                WorkoutMinutes = 0,
                WorkoutSeconds = 45,
                RestMinutes = 0,
                RestSeconds = 15,
                Rounds = 10,
                Category = "Dehnung"
            }
        };
    }

    public static void SaveWorkouts(List<WorkoutPreset> workouts)
    {
        try
        {
            var json = JsonSerializer.Serialize(workouts, new JsonSerializerOptions { WriteIndented = true });
            Preferences.Set("WorkoutPresets", json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Speichern der Workouts: {ex.Message}");
        }
    }

    public static List<WorkoutPreset> LoadWorkouts()
    {
        try
        {
            var json = Preferences.Get("WorkoutPresets", "");
            if (!string.IsNullOrEmpty(json))
            {
                var workouts = JsonSerializer.Deserialize<List<WorkoutPreset>>(json);
                return workouts ?? GetDefaultWorkouts();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Fehler beim Laden der Workouts: {ex.Message}");
        }
        
        return GetDefaultWorkouts();
    }
}
