using Admin.Domain.Contracts.Products;
using Admin.Domain.Repository.Abstractions;
using Building.Base.DTOs;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Persistence.Database.RateIt;

namespace Products.Persistence.Database.Repositories;

public sealed class RepositoryProducts(DbContextRateIt dbContext, ILogger<RepositoryProducts> logger) : Repository<DbContextRateIt, Product>(dbContext), IRepositoryProducts
{
    public async Task<bool> ProductsExistsByNameAsync(string name, int key = 0)
        => await (from Product p in dbContext.Products
                  where p.PrdId != key && p.PrdName == name
                  select p).AnyAsync();
    public async Task<IEnumerable<Product>> ProductsGetAllAsync()
    {
        IQueryable<Product> query;

        query = (from Product p in dbContext.Products.Include(p => p.Cat)
                 select p);

        return await query.ToArrayAsync().ConfigureAwait(false);
    }
    public async Task<Product> ProductsGetAsync(int key)
    {
        IQueryable<Product> query;

        query = (from Product p in dbContext.Products.Include(p => p.Cat)
                 where p.PrdId == key
                 select p);

        return await query.FirstOrDefaultAsync().ConfigureAwait(false);
    }
    public IQueryable<Product> ProductsGetQuery(string term, int size)
        => (from Product p in dbContext.Products
            where EF.Functions.Like(p.PrdName, $"%{term}%")
            select p).Take(size);
    public async Task<IEnumerable<Product>> ProductsGetPageAsync(DTOTableRequest request)
    {
        IEnumerable<Product> result;
        IQueryable<Product> query;
        Dictionary<string, string> mapper;

        mapper = new()
        {
            { "id", nameof(Product.PrdId) },
            { "name", nameof(Product.PrdName) },
            //{ "price", nameof(Product.PrdPrice) },
            { "category", nameof(Product.Cat.CatName) }
        };

        query = (from Product p in dbContext.Products select p);
        query = query.Sorting(mapper, request.ColumnsSorting).Paging(request.PageIndex, request.PageSize);
        result = await query.ToArrayAsync().ConfigureAwait(false);

        return result;
    }
    public async Task<int> ProductsGetPageFilteringCountAsync(DTOTableRequest<DTOTableFilteringProducts> request)
    {
        int result;
        IQueryable<Product> query;

        query = (from Product p in dbContext.Products.Include(p => p.Cat)
                 where p.PrdName.IndexOf(request.Filtering.Filter) != -1
                 select p);
        result = await query.CountAsync().ConfigureAwait(false);

        return result;
    }
    public async Task<IEnumerable<Product>> ProductsGetPageFilteringAsync(DTOTableRequest<DTOTableFilteringProducts> request)
    {
        IEnumerable<Product> result;
        IQueryable<Product> query;
        Dictionary<string, string> mapper;

        mapper = new()
        {
            { "id", nameof(Product.PrdId) },
            { "name", nameof(Product.PrdName) },
            //{ "price", nameof(Product.PrdPrice) },
            { "category", $"{nameof(Product.Cat)}.{nameof(Product.Cat.CatName)}" }
        };

        query = ProductsGetQuery(request);
        query = query.Sorting(mapper, request.ColumnsSorting).Paging(request.PageIndex, request.PageSize);
        result = await query.ToArrayAsync().ConfigureAwait(false);

        return result;
    }

    public async Task<IEnumerable<Category>> CategoryGetEnabledAsync(int? categoryId)
    {
        IQueryable<Category> query;

        query = from Category c in dbContext.Categories
                select c;

        var predicate = PredicateBuilder.New<Category>(c => c.CatEnabled);
        query = query.Where(predicate.Or(c => c.CatId == categoryId));

        return await query.ToArrayAsync().ConfigureAwait(false);
    }

    public enum Sequences { }

    private IQueryable<Product> ProductsGetQuery(DTOTableRequest<DTOTableFilteringProducts> request)
        => from Product p in dbContext.Products.Include(p => p.Cat)
           where p.PrdName.IndexOf(request.Filtering.Filter) != -1
           select p;

    private readonly ILogger<RepositoryProducts> logger = logger;
}