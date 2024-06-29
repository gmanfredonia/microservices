namespace Domain.Contracts.Base;

public class DTOTableRequest<TFiltering> : DTOTableRequest
{
    public TFiltering Filtering { get; set; }
}
