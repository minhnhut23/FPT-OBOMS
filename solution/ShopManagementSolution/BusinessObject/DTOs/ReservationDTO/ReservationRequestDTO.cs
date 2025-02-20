using BusinessObject.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.ReservationDTO
{
    public class ReservationRequestDTO
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Shop ID is required.")]
        public Guid ShopId { get; set; }

        [Required(ErrorMessage = "Table ID is required.")]
        public Guid TableId { get; set; }

        [Required(ErrorMessage = "Reservation time is required.")]
        [FutureDate(ErrorMessage = "Reservation time must be in the future.")]
        public DateTime ReservationTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be greater than zero.")]
        public int NumberOfGuests { get; set; }

        public string? SpecialRequests { get; set; }
    }
}
