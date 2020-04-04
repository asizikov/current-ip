using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CurrentIp.Web.Filters {
  public class OperationCancelledExceptionFilter : IExceptionFilter {
    private readonly ILogger _logger;

    public OperationCancelledExceptionFilter(ILogger<OperationCancelledExceptionFilter> logger) {
      _logger = logger;
    }

    public void OnException(ExceptionContext context) {
      if (context.Exception is OperationCanceledException) {
        _logger.LogInformation("Request was cancelled");
        context.ExceptionHandled = true;
        context.Result = new StatusCodeResult(499); //Client Closed Request
      }
    }
  }
}