using System.Linq;
using Infra.Static;
using UnityEngine;
using Object = System.Object;

public class GameLog
{
    private static string GenerateLogMessage<T>(T domain, Object message)
    {
        string logMessage = $"Domain: {domain.GetType().Name}\nType: {message.GetType().Name}\n";

        string jsonMessage = JsonUtility.ToJson(message, true);
        if (jsonMessage != "{}")
        {
            logMessage += $"Message: {jsonMessage}\n";
        }
        else
        {
            logMessage += $"Message: {message}\n";
        }

        return logMessage;
    }
    public static void Debug<T>(T domain, Object message, UnityEngine.Object context = null)
    {
        if (!Environments.IsDevelopment) return;
        UnityEngine.Debug.Log(GenerateLogMessage<T>(domain, message), context);
    }
    
    public static void Warning<T>(T domain, Object message, UnityEngine.Object context = null)
    {
        if (!Environments.IsDevelopment) return;
        UnityEngine.Debug.LogWarning(GenerateLogMessage<T>(domain, message), context);
    }
    
    public static void Error<T>(T domain, Object message, UnityEngine.Object context = null)
    {
        if (!Environments.IsDevelopment) return;
        UnityEngine.Debug.LogError(GenerateLogMessage<T>(domain, message), context);
    }
}