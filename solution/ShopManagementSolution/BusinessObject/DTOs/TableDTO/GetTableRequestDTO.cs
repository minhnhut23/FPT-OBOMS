using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.TableDTO
{
    public class GetTableRequestDTO
    {
        public string? Status { get; set; }
        public string? TableNumber { get; set; }
        public int? MinCapacity { get; set; }
        public int? MaxCapacity { get; set; }
        public Guid? TypeId { get; set; }
        public string SortBy { get; set; } = "TableNumber";
        public string SortOrder { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 2;
    }


}
