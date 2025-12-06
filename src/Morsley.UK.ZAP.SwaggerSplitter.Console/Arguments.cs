namespace Morsley.UK.ZAP.SwaggerSplitter.Console;

internal static class Arguments
{
    internal static void Gather(ref string? swaggerUrl)
    {
        if (string.IsNullOrWhiteSpace(swaggerUrl))
        {
            Display.Normal("Please enter the Swagger URL: ");

            swaggerUrl = System.Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(swaggerUrl))
            {
                Display.Warning("Swagger URL cannot be empty.");
            }
        }
    }

    internal static async Task<string?> GetJson(string url)
    {
        Display.Blank();
        Display.Mute("Getting JSON...");

        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            Display.Normal($"Fetching JSON from: {url}");

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            Display.Good($"Successfully retrieved {json.Length} characters");

            return json;
        }
        catch (Exception)
        {
            Display.Bad("Failed to get JSON!");
            Environment.Exit(1);
        }

        return null;
    }

    internal static void Output(string? swaggerUrl)
    {
        if (!string.IsNullOrWhiteSpace(swaggerUrl))
        {
            Display.Normal($"Swagger URL: '{swaggerUrl}'");
        }
    }

    internal static void Process(string[] args, ref string? swaggerUrl)
    {
        Display.Blank();
        Display.Mute("Do we have arguments passed in?");

        if (!args.Any())
        {
            Display.Normal("No");
        }
        else
        {
            Display.Normal("Yes");

            Display.Blank();
            Display.Mute("Processing...");

            for (var i = 0; i < args.Length; i++)
            {
                // Check for --swaggerUrl=value format
                if (args[i].StartsWith("--swaggerUrl=", StringComparison.OrdinalIgnoreCase))
                {
                    swaggerUrl = args[i].Substring("--swaggerUrl=".Length);

                    Display.Good($"Found 'swaggerUrl' --> {swaggerUrl}");

                    break;
                }
                // Check for --swaggerUrl value format
                else if (args[i].Equals("--swaggerUrl", StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 < args.Length)
                    {
                        swaggerUrl = args[i + 1];

                        Display.Good($"Found 'swaggerUrl' --> {swaggerUrl}");

                        break;
                    }
                }
            }
        }
    }

    internal static void Verify(string? swaggerUrl)
    {
        if (string.IsNullOrWhiteSpace(swaggerUrl))
        {
            Display.Bad("Cannot determine the Swagger URL, so exiting...");
            Environment.Exit(1);
        }
    }
}