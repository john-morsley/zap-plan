namespace SimpleAuthApi.Middleware;

public class RequestInspectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestInspectionMiddleware> _logger;

    public RequestInspectionMiddleware(RequestDelegate next, ILogger<RequestInspectionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        var logMessage = new System.Text.StringBuilder();
        logMessage.AppendLine("=");
        logMessage.AppendLine("==");
        logMessage.AppendLine("===");
        logMessage.AppendLine(@"========== Incoming Request ========== \/");
        logMessage.AppendLine($"Method: {request.Method}");
        logMessage.AppendLine($"Path: {request.Path}");
        logMessage.AppendLine($"QueryString: {request.QueryString}");
        logMessage.AppendLine($"Protocol: {request.Protocol}");
        logMessage.AppendLine($"Scheme: {request.Scheme}");
        logMessage.AppendLine($"Host: {request.Host}");
        logMessage.AppendLine($"ContentType: {request.ContentType}");
        logMessage.AppendLine($"ContentLength: {request.ContentLength}");
        logMessage.AppendLine(@"========== Incoming Request ========== /\");
        logMessage.AppendLine("===");
        logMessage.AppendLine("==");
        logMessage.AppendLine("=");
        logMessage.AppendLine("==");
        logMessage.AppendLine("===");
        logMessage.AppendLine(@"========== Request Headers ========== \/");
        foreach (var header in request.Headers)
        {
            logMessage.AppendLine($"Header: {header.Key} = {string.Join(", ", header.Value.ToArray())}");
        }
        logMessage.AppendLine(@"========== Request Headers ========== /\");
        logMessage.AppendLine("===");
        logMessage.AppendLine("==");
        logMessage.AppendLine("=");

        _logger.LogInformation("{RequestDetails}", logMessage.ToString());

        await _next(context);
    }
}