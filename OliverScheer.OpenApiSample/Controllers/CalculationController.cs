using Microsoft.AspNetCore.Mvc;
using OliverScheer.OpenApiSample.Models;
using System.Reflection;

namespace OliverScheer.OpenApiSample.Controllers;

[ApiController]
[Route("calculation")]
public class CalculationController(ILogger<CalculationController> logger) : ControllerBase
{
    private readonly ILogger<CalculationController> _logger = logger;

    [HttpPost]
    [Route("add")]
    public IActionResult Add([FromBody] AddRequest request)
    {
        int result = request.Value1 + request.Value2;
        _logger.LogDebug("Method: {methodName}, Value1: {value1}, Value2: {value2}, Result: {result}", MethodBase.GetCurrentMethod()?.Name ?? string.Empty, request.Value1, request.Value2, result);
        return Ok(new { result });
    }

    [HttpPost]
    [Route("subtract")]
    public IActionResult Subtract([FromBody] SubtractRequest request)
    {
        int result = request.Value1 - request.Value2;
        _logger.LogDebug("Method: {methodName}, Value1: {value1}, Value2: {value2}, Result: {result}", MethodBase.GetCurrentMethod()?.Name ?? string.Empty, request.Value1, request.Value2, result);
        return Ok(new { result });
    }

    [HttpPost]
    [Route("multiply")]
    public IActionResult Multiply([FromBody] MultiplyRequest request)
    {
        int result = request.Value1 * request.Value2;
        _logger.LogDebug("Method: {methodName}, Value1: {value1}, Value2: {value2}, Result: {result}", MethodBase.GetCurrentMethod()?.Name ?? string.Empty, request.Value1, request.Value2, result);
        return Ok(new { result });
    }

    [HttpPost]
    [Route("divide")]
    public IActionResult Divide([FromBody] DivideRequest request)
    {
        double result;
        if (request.Value2 == 0)
        {
            string errorMessage = "Only Chuck Norris can divide by zero.";
            _logger.LogError("Method: {methodName}, Value1: {value1}, Value2: {value2}, Error: {errorMessage}", MethodBase.GetCurrentMethod()?.Name ?? string.Empty, request.Value1, request.Value2, errorMessage);
            return BadRequest(errorMessage);
        }

        result = request.Value1 / request.Value2;
        _logger.LogDebug("Method: {methodName}, Value1: {value1}, Value2: {value2}, Result: {result}", MethodBase.GetCurrentMethod()?.Name ?? string.Empty, request.Value1, request.Value2, result);
        return Ok(new { result });
    }

    [HttpGet]
    [Route("randomvalue")]
    public IActionResult RandomValue(int min = 0, int max = 100)
    {
        Random rnd = new();
        var result = rnd.Next(min, max);
        _logger.LogDebug("Method: {methodName}, Value1: {value1}, Value2: {value2}, Result: {result}", MethodBase.GetCurrentMethod()?.Name ?? string.Empty, min, max, result);
        return Ok(new { result });
    }

    [HttpPost]
    [Route("randomvalueinrange")]
    public IActionResult RandomValue([FromBody] RandomValueRequest request)
    {
        Random rnd = new();
        var result = rnd.Next(request.Min, request.Max);
        _logger.LogDebug("Method: {methodName}, Value1: {value1}, Value2: {value2}, Result: {result}", MethodBase.GetCurrentMethod()?.Name ?? string.Empty, request.Min, request.Max, result);
        return Ok(new { result });
    }
}
