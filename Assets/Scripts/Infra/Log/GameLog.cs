using System;
using System.Linq;
using Infra.Static;

public class GameLog
{
    private static string GenerateLogMessage<T>(T domain, Object message)
    {
        var attributes = message.GetType().GetCustomAttributes(false);

        string logMessage = $"Domain: {domain.GetType().Name}\nMessage Type: {message.GetType().Name}\n";

        if (attributes.Any())
        {
            logMessage += "Attributes:\n";
            foreach (var attribute in attributes)
            {
                logMessage += $"- {attribute.GetType().Name}\n";
            }
        }
        else
        {
            logMessage += "No custom attributes found.\n";
        }
        logMessage += $"Message Content: {message}\n";

        return logMessage;
    }
    public static void Debug<T>(T domain, Object message)
    {
        if (!Environments.IsDevelopment) return;
        UnityEngine.Debug.Log(GenerateLogMessage<T>(domain, message));
    }
    
    public static void Warning<T>(T domain, Object message)
    {
        if (!Environments.IsDevelopment) return;
        UnityEngine.Debug.LogWarning(GenerateLogMessage<T>(domain, message));
    }
    
    public static void Error<T>(T domain, Object message)
    {
        if (!Environments.IsDevelopment) return;
        UnityEngine.Debug.LogError(GenerateLogMessage<T>(domain, message));
    }
}