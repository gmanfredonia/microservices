namespace Building.Base.DTOs;

public class DTOTableRequest<TFiltering> : DTOTableRequest
{
    public TFiltering Filtering { get; set; }
}
