using System;

public class GeneralGameException<T> : Exception
{
    public GeneralGameException(string message) : base(message)
    {
        GameLog.Error(this.GetType(), message);
    }
}