namespace HR.LeaveManagement.BlazorUI.Services.Base;

public class BaseHttpService
{
    protected readonly IClient _client;

    public BaseHttpService(IClient client)
    {
        _client = client;
    }

    protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
    {
        if (exception.StatusCode == 400)
        {
            return new Response<Guid>() { 
                Message = "Invalid data submitted",
                ValidationErrors = exception.Message,
                Success = false
            };
        }

        if (exception.StatusCode == 404)
        {
            return new Response<Guid>()
            {
                Message = "The record was not found.",
                Success = false
            };
        }

        return new Response<Guid>()
        {
            Message = "Something went wrong, try again later.",
            Success = false
        };
    }
}