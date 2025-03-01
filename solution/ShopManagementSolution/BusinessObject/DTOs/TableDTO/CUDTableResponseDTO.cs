using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.TableDTO
{
    public class CUDTableResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public GetTableResponseDTO? Data { get; set; }
    }
}
