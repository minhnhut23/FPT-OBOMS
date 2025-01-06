using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.TableDTO
{
    public class GetAllTablesResponseDTO
    {
        public List<GetTableResponseDTO> Data { get; set; }
        public PaginationMetadataDTO Pagination { get; set; }
    }
}
