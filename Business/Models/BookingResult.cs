namespace Business.Models;

public class BookingResult<T> : ServiceResult
{
    public T? Result { get; set; }
}

public class BookingResult : ServiceResult
{

}
