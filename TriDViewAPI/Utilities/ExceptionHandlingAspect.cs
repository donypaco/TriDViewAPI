using TriDViewAPI.Services.Interfaces;

[Serializable]
public class ExceptionHandlingAspect : OnMethodBoundaryAspect
{
    private readonly ILogService _logService;

    public ExceptionHandlingAspect(ILogService logService)
    {
        _logService = logService;
    }
    public override void OnException(MethodExecutionArgs args)
    {
//        _logService.Error($"Exception in method: {args.Method.Name} - {args.Exception.Message}");
        await _logService.LogError("Aspect", $"Exception in method: {args.Method.Name} - {args.Exception.Message}");
    }
}
