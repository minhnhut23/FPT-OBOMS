using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.ShopDTO
{
    public class UpdateShopRequestDTO
    {
        [Required(ErrorMessage = "Shop name is required.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public TimeSpan? OpeningHours { get; set; }
        public TimeSpan? ClosingHours { get; set; }
        public decimal? Rating { get; set; }
        public string? CuisineType { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
