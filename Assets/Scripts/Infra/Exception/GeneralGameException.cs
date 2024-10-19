using System;

public class GeneralGameException<T> : Exception
{
    public GeneralGameException(string message, UnityEngine.Object context = null) : base(message)
    {
        GameLog.Error(this.GetType(), message, context);
    }
}