using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.BillDTO
{
    public class DeleteBillResponseDTO
    {
        public bool IsDeleted { get; set; }
        public string Message { get; set; }
    }
}
