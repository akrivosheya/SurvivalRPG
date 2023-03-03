using System;

public class NoSuchAudioClipException : Exception
{
    public NoSuchAudioClipException(int clipIndex) : 
    base("There is not audio clip with index " + clipIndex + "")
    {
    }
}
