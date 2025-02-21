﻿namespace Building.Base.DTOs;

public class DTOTableRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<DTOTableColumnSorting> ColumnsSorting { get; set; }
}