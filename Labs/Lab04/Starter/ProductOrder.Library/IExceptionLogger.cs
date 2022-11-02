namespace ProductOrder.Library;

public interface IExceptionLogger
{
    void LogException(Exception exception);
}
