using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.ProductDTO
{
    public class GetProductRequestDTO
    {
        public string? Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MinPrice must be greater than or equal to 0.")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MaxPrice must be greater than or equal to 0.")]
        public decimal? MaxPrice { get; set; }

        [StringLength(100, ErrorMessage = "Category name can't exceed 100 characters.")]
        public string? Category { get; set; }

        public bool? IsAvailable { get; set; }

        // Pagination parameters
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }

}
