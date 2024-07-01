namespace Building.Base.DTOs;

public class DTOTableResponse<TRow> : DTOTablePage<TRow>
{
    public int TotalCount { get; set; }
}
