using AutoMapper;
using BusinessObject.DTOs.TableDTO;
using BusinessObject.Models;


namespace ShopManagementService.Utils {
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping Table to GetTableResponseDTO
            CreateMap<Table, GetTableResponseDTO>()
                .ForMember(dest => dest.TableType, opt => opt.Ignore()); // Ignore TableType (sẽ xử lý riêng)

            // Mapping CreateTableRequestDTO to Table
            CreateMap<CreateTableRequestDTO, Table>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Mapping UpdateTableRequestDTO to Table
            CreateMap<UpdateTableRequestDTO, Table>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}