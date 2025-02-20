using BusinessObject.DTOs.ReservationDTO;
using BusinessObject.Models;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;

namespace ShopManagementService.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ReservationDAO _reservationDao;

        public ReservationRepository(ReservationDAO reservationDao) => _reservationDao = reservationDao;

        public Task<ReservationResponseDTO> CreateReservation(ReservationRequestDTO request)
            => _reservationDao.CreateReservation(request);

        public Task<List<Reservation>> GetAllReservations()
            => _reservationDao.GetAllReservations();

        public Task<List<Reservation>> GetReservationsByCustomer(Guid customerId)
            => _reservationDao.GetReservationsByCustomer(customerId);

        public Task<Reservation?> GetReservationById(Guid id)
            => _reservationDao.GetReservationById(id);

        public Task<ReservationResponseDTO> UpdateReservation(Guid id, ReservationRequestDTO request)
            => _reservationDao.UpdateReservation(id, request);

        public Task<ReservationResponseDTO> DeleteReservation(Guid id)
            => _reservationDao.DeleteReservation(id);

        public Task<ReservationResponseDTO> CancelReservation(Guid id)
            => _reservationDao.CancelReservation(id);
    }
}
