//using AutoMapper;
using Domain.Contracts.Products;
using Domain.Entities;

namespace Services;

/*internal class MapperProducts : Profile
{
    public MapperProducts()
    {
        this.RecognizePrefixes(["Prd"]);
        this.RecognizeDestinationPrefixes(["Prd"]);
        CreateMap<Product, DTOProduct>().ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CatId))
                                        .ReverseMap()
                                        .AfterMap((src, dest, context) =>
                                        {
                                            if (src.Id == 0)
                                            {
                                                dest.PrdInsertDate = DateTime.Now;
                                                dest.PrdLastUpdate = null;
                                            }
                                            else
                                            {
                                                dest.PrdInsertDate = (DateTime)context.Items["insertDate"];
                                                dest.PrdLastUpdate = DateTime.Now;
                                            }
                                        });
        CreateMap<Product, DTOProductRow>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PrdId))
                                           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PrdName))
                                           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PrdDescription))                                                                                      
                                           .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Cat.CatName));
        CreateMap<Product, DTOProductItem>().ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.PrdId))
                                            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PrdName))
                                            .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.PrdEnabled));
        CreateMap<Category, DTOCategoryItem>().ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.CatId))
                                              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.CatName))
                                              .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.CatEnabled));
    }
}
*/

// Mappings/MappingConfig.cs
using Mapster;

public static class MappingConfig
{
    public static void RegisterMappings()
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

