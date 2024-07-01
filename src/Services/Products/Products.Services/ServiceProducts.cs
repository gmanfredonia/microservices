using Admin.Domain.Contracts.Products;
using Admin.Domain.Repository.Abstractions;
using Building.Base.DTOs;
using Building.Base.Exceptions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Services.Abstractions;

namespace Products.Services;

public sealed class ServiceProducts(IConfiguration configuration, ILogger<ServiceProducts> logger, IMapper mapper, IStringLocalizer localizer,
                                    IRepositoryManager repositoryManager) : ServiceBase(configuration, logger, mapper, localizer), IServiceProducts
{
    public async Task<IEnumerable<DTOProductRow>> ProductsGetAllAsync()
    {
        IEnumerable<Product> items;

        await using IRepositoryProducts repository = repositoryManager.Reading.ProductsGetInstance();

        items = await repository.ProductsGetAllAsync().ConfigureAwait(false);

        return items.Adapt<IEnumerable<DTOProductRow>>();
    }
    public async Task<DTOProduct> ProductsGetAsync(int key)
    {
        Product item;

        await using IRepositoryProducts repository = repositoryManager.Reading.ProductsGetInstance();

        item = await repository.ProductsGetAsync(key).ConfigureAwait(false);

        return item.Adapt<DTOProduct>();
    }
    public async Task<IEnumerable<DTOProductItem>> ProductsGetAsync(string term)
    {
        IQueryable<DTOProductItem> query;
        IEnumerable<DTOProductItem> items;

        await using IRepositoryProducts repository = repositoryManager.Reading.ProductsGetInstance();

        query = mapper.From(repository.ProductsGetQuery(term, 10)).ProjectToType<DTOProductItem>();
        items = await repository.MaterializeDataAsync(query).ConfigureAwait(false);

        return items;
    }

    public async Task<DTOTableResponse<DTOProductRow>> ProductsGetPageAsync(DTOTableRequest request)
    {
        Task<IEnumerable<Product>> taskPage;
        Task<int> taskCount;

        await using IRepositoryProducts repositoryPage = repositoryManager.Reading.ProductsGetInstance();
        await using IRepositoryProducts repositoryCount = repositoryManager.Reading.ProductsGetInstance();

        taskPage = repositoryPage.ProductsGetPageAsync(request);
        taskCount = repositoryCount.CountAsync();

        await Task.WhenAll(taskPage, taskCount).ConfigureAwait(false);

        return new DTOTableResponse<DTOProductRow>()
        {
            Rows = (await taskPage).Adapt<IEnumerable<DTOProductRow>>(),
            FilteredCount = await taskCount,
            TotalCount = await taskCount
        };
    }

    public async Task<bool> ProductsExistsByNameAsync(string name)
    {
        await using IRepositoryProducts repository = repositoryManager.Reading.ProductsGetInstance();

        return await repository.ProductsExistsByNameAsync(name).ConfigureAwait(false);
    }
    public async Task<DTOTableResponse<DTOProductRow>> ProductsGetPageFilteringAsync(DTOTableRequest<DTOTableFilteringProducts> request)
    {
        Task<IEnumerable<Product>> taskPage;
        Task<int> taskFilteredCount, taskCount;

        await using IRepositoryProducts repositoryPage = repositoryManager.Reading.ProductsGetInstance();
        await using IRepositoryProducts repositoryFilteredCount = repositoryManager.Reading.ProductsGetInstance();
        await using IRepositoryProducts repositoryCount = repositoryManager.Reading.ProductsGetInstance();

        // await Task.Delay(2 * 1000);

        taskPage = repositoryPage.ProductsGetPageFilteringAsync(request);
        taskFilteredCount = repositoryFilteredCount.ProductsGetPageFilteringCountAsync(request);
        taskCount = repositoryCount.CountAsync();

        await Task.WhenAll(taskPage, taskFilteredCount, taskCount).ConfigureAwait(false);

        return new DTOTableResponse<DTOProductRow>()
        {
            Rows = (await taskPage).Adapt<IEnumerable<DTOProductRow>>(),
            FilteredCount = await taskFilteredCount,
            TotalCount = await taskCount
        };
    }

    public async Task<int> ProductsCreateAsync(DTOProduct request)
    {
        Product product;

        if (await repositoryManager.Writing.Products.ProductsExistsByNameAsync(request.Name).ConfigureAwait(false))            
            throw new ModelStateError(nameof(request.Name), localizer["messageItemAlreadyExists"]);

        product = request.Adapt<Product>();        
        await repositoryManager.Writing.Products.AddAsync(product).ConfigureAwait(false);
        await repositoryManager.Writing.SaveChangesAsync().ConfigureAwait(false);

        return product.PrdId;
    }

    public async Task ProductsEditAsync(DTOProduct request)
    {
        Product product = await repositoryManager.Writing.Products.GetByKeyAsync(request.Id);

        if (await repositoryManager.Writing.Products.ProductsExistsByNameAsync(request.Name, request.Id).ConfigureAwait(false))        
            throw new ModelStateError(nameof(request.Name), localizer["messageItemAlreadyExists"]);                    
        
        product = request.BuildAdapter().AddParameters("insertDate", product.PrdInsertDate).AdaptToType<Product>();        
        repositoryManager.Writing.Products.Update(product);
        await repositoryManager.Writing.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task ProductsRemoveAsync(int request)
    {
        Product product;

        product = new Product() { PrdId = request };
        repositoryManager.Writing.Products.Attach(product);
        repositoryManager.Writing.Products.Remove(product);
        await repositoryManager.Writing.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<DTOCategoryItem>> CategoriesGetEnabledAsync(int? key)
    {
        IEnumerable<Category> items;

        await using IRepositoryProducts repository = repositoryManager.Reading.ProductsGetInstance();

        items = await repository.CategoryGetEnabledAsync(key).ConfigureAwait(false);

        return items.Adapt<IEnumerable<DTOCategoryItem>>();
    }


    private readonly IRepositoryManager repositoryManager = repositoryManager;
}