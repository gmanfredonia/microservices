using Domain.Contracts.Base;
using Domain.Contracts.Products;

namespace Services.Abstractions;

public interface IServiceProducts
{
    public Task<IEnumerable<DTOProductRow>> ProductsGetAllAsync();
    public Task<DTOProduct> ProductsGetAsync(int key);
    public Task<IEnumerable<DTOProductItem>> ProductsGetAsync(string term);
    public Task<DTOTableResponse<DTOProductRow>> ProductsGetPageAsync(DTOTableRequest request);
    public Task<DTOTableResponse<DTOProductRow>> ProductsGetPageFilteringAsync(DTOTableRequest<DTOTableFilteringProducts> request);

    public Task<int> ProductsCreateAsync(DTOProduct request);
    public Task ProductsEditAsync(DTOProduct request);
    public Task ProductsRemoveAsync(int request);

    public Task<IEnumerable<DTOCategoryItem>> CategoriesGetEnabledAsync(int? key);
}