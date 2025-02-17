using BusinessObject.DTOs.ReservationDTO;
using BusinessObject.DTOs.TableTypeDTO;
using BusinessObject.Models;

namespace ShopManagementService.IRepositories
{
    public interface IReservationRepository
    {
            Task<ReservationResponseDTO> CreateReservation(ReservationRequestDTO request);
            Task<List<Reservation>> GetAllReservations();
            Task<List<Reservation>> GetReservationsByCustomer(Guid customerId);
            Task<Reservation?> GetReservationById(Guid id);
            Task<ReservationResponseDTO> UpdateReservation(Guid id, ReservationRequestDTO request);
            Task<ReservationResponseDTO> DeleteReservation(Guid id);
            Task<ReservationResponseDTO> CancelReservation(Guid id);
        }
    
}
