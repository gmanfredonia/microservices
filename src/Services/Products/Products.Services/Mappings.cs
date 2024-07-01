using Admin.Domain.Contracts.Products;
using Mapster;
using Products.Domain.Entities;

namespace Products.Services;

public static class Mappings
{
    public static void Register()
    {
        TypeAdapterConfig<Product, DTOProduct>.NewConfig()
                .NameMatchingStrategy(NameMatchingStrategy.ConvertDestinationMemberName(name => "Prd" + name))
                .Map(dest => dest.CategoryId, src => src.CatId);
        TypeAdapterConfig<DTOProduct, Product>.NewConfig()
                .NameMatchingStrategy(NameMatchingStrategy.ConvertDestinationMemberName(name => name.Remove(0, 3)))
                .Map(dest => dest.CatId, src => src.CategoryId)
        .AfterMapping((src, dest) =>
         {
             if (src.Id == 0)
             {
                 dest.PrdInsertDate = DateTime.Now;
                 dest.PrdLastUpdate = null;
             }
             else
             {
                 dest.PrdInsertDate = (DateTime)MapContext.Current.Parameters["insertDate"];
                 dest.PrdLastUpdate = DateTime.Now;
             }
         });

        TypeAdapterConfig<Product, DTOProductRow>.NewConfig()
                                                 .NameMatchingStrategy(NameMatchingStrategy.ConvertDestinationMemberName(name => "Prd" + name))
                                                 .Map(dest => dest.Category, src => src.Cat.CatName);
        TypeAdapterConfig<Product, DTOProductItem>.NewConfig()
                                                  .Map(dest => dest.Key, src => src.PrdId)
                                                  .Map(dest => dest.Description, src => src.PrdName)
                                                  .Map(dest => dest.Enabled, src => src.PrdEnabled);
        TypeAdapterConfig<Category, DTOCategoryItem>.NewConfig()
                                                    .Map(dest => dest.Key, src => src.CatId)
                                                    .Map(dest => dest.Description, src => src.CatName)
                                                    .Map(dest => dest.Enabled, src => src.CatEnabled);
    }
}

