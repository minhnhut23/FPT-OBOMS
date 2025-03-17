using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.BillDTO
{
    public class BillResponseStatusDTO
    {
        public bool Success { get; set; }  
        public string Message { get; set; }
        public BillResponseDTO? Data { get; set; } 
    }

}
