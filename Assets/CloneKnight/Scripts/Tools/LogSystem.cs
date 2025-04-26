using System.Diagnostics;

public static class LogSystem
{

    static string GetFormattedMessage(string message)
    {
        StackTrace stackTrace = new();
        StackFrame frame = stackTrace.GetFrame(1); // 0 = LogSystem.LogError'ın kendisi, 1 = çağıran yer
        var method = frame.GetMethod();
        string className = method.DeclaringType.Name;
        string methodName = method.Name;

        return $"{className}.cs/{methodName}(): {message}";
    }
    public static void LogError(string message)
    {
        UnityEngine.Debug.LogError(GetFormattedMessage(message));
    }

    public static void Log(string message)
    {
        UnityEngine.Debug.Log(GetFormattedMessage(message));
    }


}
