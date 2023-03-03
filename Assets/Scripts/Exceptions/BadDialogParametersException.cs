using System;

public class BadDialogParametersException : Exception
{
    public BadDialogParametersException(string message) : 
    base(message)
    {
    }
}
