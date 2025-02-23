namespace SampSharp.Entities.SAMP;

public class InvalidPlayerNameException : Exception
{
    public InvalidPlayerNameException() { }
    public InvalidPlayerNameException(string message) : base(message) { }
    public InvalidPlayerNameException(string message, Exception inner) : base(message, inner) { }
}