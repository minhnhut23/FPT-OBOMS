using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.ShopDTO
{
    public class CreateShopRequestDTO
    {

        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string OpeningHours { get; set; } = null!; 
        public string ClosingHours { get; set; } = null!;
        public string CuisineType { get; set; } = null!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public Guid SubscriptionId { get; set; }

    }
}
