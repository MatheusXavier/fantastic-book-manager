namespace Book.Domain.Results;

public class ErrorDetail
{
    private readonly List<ErrorItem> _errorItems;

    public int Status { get; }

    public ErrorMessage Message { get; }

    public IReadOnlyCollection<ErrorItem> Errors => _errorItems;

    public ErrorDetail(int status, ErrorMessage message)
    {
        Status = status;
        Message = message;
        _errorItems = new List<ErrorItem>();
    }

    public ErrorDetail AddError(ErrorItem? errorItem)
    {
        if (errorItem == null)
        {
            return this;
        }

        _errorItems.Add(errorItem);
        return this;
    }
}
