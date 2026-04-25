using UnityEngine;
using System.IO;

public static class GraphicsConfigManager
{
    private static GraphicsConfig _cached;
    private static GraphicsConfig _workingCopy;

    private static string filePath => Path.Combine(Application.persistentDataPath, "graphics.json");

    public static GraphicsConfig GetWorkingCopy()
    {
        if (_cached == null)
        {
            _cached = LoadConfig();
        }
        if (_workingCopy == null)
        {
            _workingCopy = _cached.Clone();
        }
        return _workingCopy;
    }

    public static void SaveAndApply()
    {
        _cached = GetWorkingCopy().Clone();
        string json = JsonUtility.ToJson(_cached, true);
        File.WriteAllText(filePath, json);
        ApplyCurrentSettings();
    }

    public static void ApplyCurrentSettings()
    {
        var config = GetWorkingCopy();
        Screen.SetResolution(config.resolutionWidth, config.resolutionHeight, config.fullscreen);
    }

    public static void DiscardChanges()
    {
        _workingCopy = null;
    }

    private static GraphicsConfig LoadConfig()
    {
        if (File.Exists(filePath))
        {
            return JsonUtility.FromJson<GraphicsConfig>(File.ReadAllText(filePath));
        }

        return new GraphicsConfig
        {
            resolutionWidth = 800,
            resolutionHeight = 800,
            fullscreen = false
        };
    }
}