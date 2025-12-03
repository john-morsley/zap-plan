namespace SimpleAuthApi.Middleware;

public class RequestResponseInspectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseInspectionMiddleware> _logger;

    public RequestResponseInspectionMiddleware(RequestDelegate next, ILogger<RequestResponseInspectionMiddleware> logger)
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

        // Capture the original response body stream
        var originalBodyStream = context.Response.Body;

        using var responseBody = new System.IO.MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        // Log response details
        var responseLog = new System.Text.StringBuilder();
        responseLog.AppendLine("=");
        responseLog.AppendLine("==");
        responseLog.AppendLine("===");
        responseLog.AppendLine(@"========== Outgoing Response ========== \/");
        responseLog.AppendLine($"StatusCode: {context.Response.StatusCode}");
        responseLog.AppendLine($"ContentType: {context.Response.ContentType}");
        responseLog.AppendLine($"ContentLength: {context.Response.ContentLength}");
        responseLog.AppendLine(@"========== Outgoing Response ========== /\");
        responseLog.AppendLine("===");
        responseLog.AppendLine("==");
        responseLog.AppendLine("=");
        responseLog.AppendLine("==");
        responseLog.AppendLine("===");
        responseLog.AppendLine(@"========== Response Headers ========== \/");
        foreach (var header in context.Response.Headers)
        {
            responseLog.AppendLine($"Header: {header.Key} = {string.Join(", ", header.Value.ToArray())}");
        }
        responseLog.AppendLine(@"========== Response Headers ========== /\");
        responseLog.AppendLine("===");
        responseLog.AppendLine("==");
        responseLog.AppendLine("=");
        
        // Read response body
        responseBody.Seek(0, System.IO.SeekOrigin.Begin);
        var responseBodyText = await new System.IO.StreamReader(responseBody).ReadToEndAsync();
        if (!string.IsNullOrEmpty(responseBodyText))
        {
            var preview = responseBodyText.Length > 500 ? responseBodyText.Substring(0, 500) + "..." : responseBodyText;
            responseLog.AppendLine("==");
            responseLog.AppendLine("===");
            responseLog.AppendLine(@"========== Response Body ========== \/");
            responseLog.AppendLine(preview);
            responseLog.AppendLine(@"========== Response Body ========== /\");
            responseLog.AppendLine("===");
            responseLog.AppendLine("==");
            responseLog.AppendLine("=");
        }

        _logger.LogInformation("{ResponseDetails}", responseLog.ToString());

        // Copy the response body back to the original stream
        responseBody.Seek(0, System.IO.SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    }
}