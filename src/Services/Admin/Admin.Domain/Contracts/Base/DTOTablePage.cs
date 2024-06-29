namespace Admin.Domain.Contracts.Base;

public class DTOTablePage<TRow>
{
    public IEnumerable<TRow> Rows { get; set; }
    public int FilteredCount { get; set; }
}
