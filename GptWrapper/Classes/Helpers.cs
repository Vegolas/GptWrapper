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

    public static async Task<string> GetFocusedText()
    {
        Clipboard.Clear();

        await Task.Delay(50);
        SendKeys.Send("^{c}");
        await Task.Delay(150);

        return Clipboard.GetText();
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
    
    public static int ProcessRefactorText(ref string refactorText)
    {
        const string searchString = "@returnPoint";
        int index = -1;
        if (refactorText.Contains(searchString))
        {
            index = refactorText.IndexOf(searchString);
            refactorText = refactorText.Replace(searchString, string.Empty);
        }
        return index;
    }

    public static async Task<string> GetTemplateWithFocusedText(string template)
    {
        var text = await Helpers.GetFocusedText();
        var askText = template.Replace("@focusedText", text);
        return askText;
    }
}
