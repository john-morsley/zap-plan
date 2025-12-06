namespace Morsley.UK.ZAP.SwaggerSplitter.Console;

internal static class SwaggerSplitter
{
    internal static Dictionary<string, string> Split(string swaggerJson)
    {
        Display.Blank();
        Display.Mute("Splitting...");

        var result = new Dictionary<string, string>();

        // Parse the JSON
        using var document = JsonDocument.Parse(swaggerJson);
        var root = document.RootElement;

        // Check if "paths" node exists
        if (!root.TryGetProperty("paths", out JsonElement pathsElement))
        {
            Display.Bad("No 'paths' node found in Swagger JSON");
            Environment.Exit(1);
        }

        try 
        {
            // Create base structure (everything except paths)
            var baseStructure = new Dictionary<string, object?>();
            foreach (var property in root.EnumerateObject())
            {
                if (property.Name != "paths")
                {
                    baseStructure[property.Name] = TraverseAndDuplicate(property.Value);
                }
            }

            // Get original title once
            string originalTitle = "API";
            if (root.TryGetProperty("info", out JsonElement infoElement) &&
                infoElement.TryGetProperty("title", out JsonElement titleElement))
            {
                originalTitle = titleElement.GetString() ?? "API";
            }

            // Create one duplicate per path
            foreach (var path in pathsElement.EnumerateObject())
            {
                // Deep clone the base structure
                var duplicate = DeepClone(baseStructure);

                // Update the title in info node
                if (duplicate.ContainsKey("info") && duplicate["info"] is Dictionary<string, object?> info)
                {
                    if (info.ContainsKey("title"))
                    {
                        info["title"] = $"{originalTitle} - Path: {path.Name}";
                    }
                }

                // Add only this single path
                var singlePath = new Dictionary<string, object?>
                {
                    [path.Name] = TraverseAndDuplicate(path.Value)
                };
                duplicate["paths"] = singlePath;

                // Serialize to JSON string
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var duplicateJsonString = JsonSerializer.Serialize(duplicate, options);

                // Add to dictionary with path as key
                result[path.Name] = duplicateJsonString;
            }
        }
        catch
        {
            Display.Bad("An unexpected error occurred processing the Swagger JSON, so exiting...");
            Environment.Exit(1);
        }

        if (result.Any())
        {            
            foreach (var item in result)
            {
                Display.Normal($"Path: '{item.Key}'");
            }
            Display.Good($"Created {result.Count} split Swagger documents");
        }
        else
        {
            Display.Bad("Nothing to split, so exiting...");
            Environment.Exit(1);
        }

        return result;
    }

    private static Dictionary<string, object?> DeepClone(Dictionary<string, object?> source)
    {
        var clone = new Dictionary<string, object?>();
        foreach (var kvp in source)
        {
            clone[kvp.Key] = DeepCloneValue(kvp.Value);
        }
        return clone;
    }

    private static object? DeepCloneValue(object? value)
    {
        if (value == null) return null;

        if (value is Dictionary<string, object?> dict)
        {
            var clonedDict = new Dictionary<string, object?>();
            foreach (var kvp in dict)
            {
                clonedDict[kvp.Key] = DeepCloneValue(kvp.Value);
            }
            return clonedDict;
        }

        if (value is List<object?> list)
        {
            var clonedList = new List<object?>();
            foreach (var item in list)
            {
                clonedList.Add(DeepCloneValue(item));
            }
            return clonedList;
        }

        // Primitive types (string, int, bool, etc.) are immutable
        return value;
    }

    private static object? TraverseAndDuplicate(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var obj = new Dictionary<string, object?>();
                foreach (var property in element.EnumerateObject())
                {
                    obj[property.Name] = TraverseAndDuplicate(property.Value);
                }
                return obj;

            case JsonValueKind.Array:
                var array = new List<object?>();
                foreach (var item in element.EnumerateArray())
                {
                    array.Add(TraverseAndDuplicate(item));
                }
                return array;

            case JsonValueKind.String:
                return element.GetString();

            case JsonValueKind.Number:
                if (element.TryGetInt32(out int intValue))
                    return intValue;
                if (element.TryGetInt64(out long longValue))
                    return longValue;
                return element.GetDouble();

            case JsonValueKind.True:
                return true;

            case JsonValueKind.False:
                return false;

            case JsonValueKind.Null:
                return null;

            default:
                return null;
        }
    }
}