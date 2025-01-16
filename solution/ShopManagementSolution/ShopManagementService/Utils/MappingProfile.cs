using AutoMapper;
using BusinessObject.DTOs.ProductDTO;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;


namespace ShopManagementService.Utils {
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Table, GetTableResponseDTO>()
                .ForMember(dest => dest.TableType, opt => opt.Ignore()); 

            CreateMap<CreateTableRequestDTO, Table>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateTableRequestDTO, Table>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<CreateProductRequestDTO, MenuItem>()
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
              .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
              .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(_ => true)); // Default: available

            // Map UpdateProductRequestDTO to MenuItem (Update)
            CreateMap<UpdateProductRequestDTO, MenuItem>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Map MenuItem to GetProductResponseDTO
            CreateMap<MenuItem, GetProductResponseDTO>();
        }
    }
}