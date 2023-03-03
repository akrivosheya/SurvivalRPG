using System;

public class OutOfGridPositionException : Exception
{
    public OutOfGridPositionException(int x, int y, int width, int height) : 
    base("Position (" + x + "; " + y + ") is out of grid (" + width + "; " + height + ")")
    {
    }
}
