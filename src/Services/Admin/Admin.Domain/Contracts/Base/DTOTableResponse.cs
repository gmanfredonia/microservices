namespace Admin.Domain.Contracts.Base;

public class DTOTableResponse<TRow> : DTOTablePage<TRow>
{
    public int TotalCount { get; set; }
}
