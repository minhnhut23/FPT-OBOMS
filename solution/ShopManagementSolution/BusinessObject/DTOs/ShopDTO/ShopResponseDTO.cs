using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.ShopDTO
{
    public class ShopResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string OpeningHours { get; set; }
        public string ClosingHours { get; set; }
        public decimal? Rating { get; set; }
        public string CuisineType { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public Guid OwnerId { get; set; }
    }
}
