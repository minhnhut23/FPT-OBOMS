using Microsoft.AspNetCore.Mvc;
using ShopManagementService.DAO;
using BusinessObject.DTOs.ReservationDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IReservationRepository reservationDAO)
        {
            _reservationRepository = reservationDAO;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequestDTO request)
        {
            var response = await _reservationRepository.CreateReservation(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetReservationsByCustomer(Guid customerId)
        {
            var reservations = await _reservationRepository.GetReservationsByCustomer(customerId);

            if (reservations.Count == 0)
            {
                return Ok(new { Message = "This customer has not made any reservations yet."});
            }

            return Ok(reservations);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(Guid id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);
            if (reservation != null)
            {
                return Ok(reservation);
            }
            return NotFound("Reservation not found.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(Guid id, [FromBody] ReservationRequestDTO request)
        {
            var response = await _reservationRepository.UpdateReservation(id, request);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(Guid id)
        {
            var response = await _reservationRepository.DeleteReservation(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelReservation(Guid id)
        {
            var response = await _reservationRepository.CancelReservation(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
