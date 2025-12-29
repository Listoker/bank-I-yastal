namespace bank_i_ystal.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}