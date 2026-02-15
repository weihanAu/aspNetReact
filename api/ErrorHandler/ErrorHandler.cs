using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class GlobalExceptionHandler : IExceptionHandler
{
  private readonly ILogger<GlobalExceptionHandler> _logger;

  public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    // 1. 记录日志（包含堆栈信息，用于排查）
    _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

    // 2. 根据异常类型确定状态码
    var statusCode = exception switch
    {
      UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401
      KeyNotFoundException => HttpStatusCode.NotFound,           // 404
      _ => HttpStatusCode.InternalServerError                    // 500
    };

    // 3. 构建返回给前端的 JSON
    var response = new ApiResponse<string>
    {
      Success = false,
      Message = "服务器内部错误，请联系管理员",
      ErrorCode = "ERR_" + statusCode.ToString().ToUpper()
    };

    // 如果是开发环境，可以把具体错误塞进 Message 里
    response.Message = exception.Message;

    httpContext.Response.StatusCode = (int)statusCode;
    await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

    return true; // 表示该异常已被处理
  }
}