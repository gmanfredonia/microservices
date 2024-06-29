namespace Admin.Domain.Contracts.Base;

public class DTOTableRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<DTOTableColumnSorting> ColumnsSorting { get; set; }
}