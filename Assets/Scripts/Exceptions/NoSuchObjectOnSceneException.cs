using System;

public class NoSuchObjectOnSceneException : Exception
{
    public NoSuchObjectOnSceneException(int objectId) : base("There is no object " + objectId + " on scene.")
    {
    }
}
