namespace Morsley.UK.ZAP.SwaggerSplitter.Console;

internal class Persistence
{
    internal static void SaveAsFiles(Dictionary<string, string> splitSwaggerJson)
    {
        Display.Blank();
        Display.Mute("Persisting JSON...");

        const string folderName = "SplitSwaggerFiles";

        try
        {
            if (Directory.Exists(folderName))
            {
                Display.Normal($"Using folder: '{folderName}'");
            }
            else
            {
                Display.Warning($"Trying to create folder: '{folderName}'");
                Directory.CreateDirectory(folderName);
                Display.Good($"Created folder: {folderName}");
            }

            foreach (var kvp in splitSwaggerJson)
            {
                var fileName = $"swagger{kvp.Key.Replace("/", "-")}.json";
                var filePath = Path.Combine(folderName, fileName);

                Display.Warning($"Trying to create file: '{fileName}'");
                File.WriteAllText(filePath, kvp.Value);
                Display.Normal($"Saved: {filePath}");
            }

            Display.Good($"Saved {splitSwaggerJson.Count} split Swagger files to {folderName}");
        }
        catch
        {
            Display.Bad("Could not persist JSON!");
        }
    }
}