using Admin.Domain.Contracts.Base;
using Admin.Domain.Contracts.Products;
using Admin.Domain.Entities;

namespace Admin.Domain.Repository.Abstractions;

public interface IRepositoryProducts : IRepository<Product> 
{
    Task<bool> ProductsExistsByNameAsync(string name, int key = 0);
    Task<IEnumerable<Product>> ProductsGetAllAsync();
    Task<Product> ProductsGetAsync(int key);                
    public IQueryable<Product> ProductsGetQuery(string term, int size);
    Task<IEnumerable<Product>> ProductsGetPageAsync(DTOTableRequest request);
    Task<int> ProductsGetPageFilteringCountAsync(DTOTableRequest<DTOTableFilteringProducts> request);
    Task<IEnumerable<Product>> ProductsGetPageFilteringAsync(DTOTableRequest<DTOTableFilteringProducts> request);
    Task<IEnumerable<Category>> CategoryGetEnabledAsync(int? categoryId);
}