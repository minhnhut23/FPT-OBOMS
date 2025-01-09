using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.TableDTO
{
    public class UpdateTableRequestDTO
    {
        [Required(ErrorMessage = "Table number is required.")]
        [StringLength(50, ErrorMessage = "Table number cannot exceed 50 characters.")]
        public string TableNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; }

        [StringLength(1000, ErrorMessage = "Location description cannot exceed 1000 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Type ID is required.")]
        public Guid TypeId { get; set; }
    }
}
