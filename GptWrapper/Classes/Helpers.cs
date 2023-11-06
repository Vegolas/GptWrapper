using System.Text;
using System.Text.Json;

namespace GptWrapper.Classes;
public static class Helpers
{
    public static bool IsValidJson(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
        {
            return false;
        }

        try
        {
            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                return true;
            }
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public static bool SaveSettings(string text, string settingsPath)
    {
        if (Helpers.IsValidJson(text) == false)
        {
            MessageBox.Show("Value provided as settings are not a valid JSON.", "Error saving settings");
            return false;
        }

        string appDirectory = AppContext.BaseDirectory;
        string filePath = Path.Combine(appDirectory, settingsPath);
        File.WriteAllText(filePath, text, Encoding.UTF8);

        return true;
    }
}
