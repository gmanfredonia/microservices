using Domain.Contracts.Base;

namespace Persistence.Database;

public static class RepositoryHelpers
{
    public static IQueryable<T> Paging<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        => query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    public static IQueryable<T> Sorting<T>(this IQueryable<T> query, IDictionary<string, string> columnsMapper, IEnumerable<DTOTableColumnSorting> columnsSorting)
    {
        bool firstOrder;
        IOrderedQueryable<T> queryOrdered;

        if (columnsSorting != null)
        {
            firstOrder = true;
            foreach (DTOTableColumnSorting item in columnsSorting)
                if (columnsMapper.TryGetValue(item.Column, out string columnName))
                    if (firstOrder)
                    {
                        switch (item.Direction)
                        {
                            case "asc":
                                query = query.OrderBy(columnName);
                                break;
                            case "desc":
                                query = query.OrderByDescending(columnName);
                                break;
                        }
                        firstOrder = false;
                    }
                    else
                    {
                        queryOrdered = query as IOrderedQueryable<T>;
                        switch (item.Direction)
                        {
                            case "asc":
                                query = queryOrdered.ThenBy(columnName);
                                break;
                            case "desc":
                                query = queryOrdered.ThenByDescending(columnName);
                                break;
                        }
                    }
                else
                    throw new KeyNotFoundException($"Column name '{item.Column}' not mapped!");
        }

        return query;
    }
}
