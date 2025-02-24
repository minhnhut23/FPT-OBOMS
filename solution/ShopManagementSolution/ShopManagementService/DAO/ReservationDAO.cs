using AutoMapper;
using BusinessObject.DTOs.ReservationDTO;
using BusinessObject.Enums;
using BusinessObject.Models;
using BusinessObject.Utils;
using Newtonsoft.Json;
using Supabase;
using static Supabase.Postgrest.Constants;

namespace ShopManagementService.DAO
{
    public class ReservationDAO
    {
        private readonly Client _client;

        public ReservationDAO(Client client)
        {
            _client = client;
        }

        public async Task<ReservationResponseDTO> CreateReservation(ReservationRequestDTO request)
        {
            try
            {
                var table = await _client.From<Table>()
                                         .Where(t => t.Id == request.TableId)
                                         .Single();

                if (table == null)
                    return new ReservationResponseDTO
                    {
                        Success = false,
                        Message = "Table not found."
                    };

                if (table.Status != "available")
                    return new ReservationResponseDTO
                    {
                        Success = false,
                        Message = "Table is not available for reservation."
                    };
                var reservation = new Reservation
                {
                    CustomerId = request.CustomerId,
                    ShopId = request.ShopId,
                    TableId = request.TableId,
                    ReservationTime = DateTime.UtcNow,
                    NumberOfGuests = request.NumberOfGuests,
                    SpecialRequests = request.SpecialRequests,
                    StatusEnum = ReservationStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _client.From<Reservation>().Insert(reservation);

                //Supabase auto generate so neeed to get newest one
                var insertedReservation = await _client.From<Reservation>()
                    .Where(r => r.CustomerId == reservation.CustomerId && r.ShopId == reservation.ShopId)
                    .Order("created_at", Supabase.Postgrest.Constants.Ordering.Descending)
                    .Single();
                table.Status = "onusing";
                await _client.From<Table>().Update(table);

                Console.WriteLine($"Inserted Reservation: {JsonConvert.SerializeObject(insertedReservation)}");

                return new ReservationResponseDTO
                {
                    Success = true,
                    Message = "Reservation created successfully.",
                    ReservationId = insertedReservation.Id
                };
            }
           /* catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }*/
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                return new ReservationResponseDTO
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }

        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            var response = await _client.From<Reservation>().Get();
            return response.Models; 
        }

        public async Task<List<Reservation>> GetReservationsByCustomer(Guid customerId)
        {
            if (customerId == Guid.Empty)
            {
                return new List<Reservation>(); 
            }

          /*  var customer = await _client.From<BusinessObject.Models.Profiles>()
                                        .Where(c => c.Id == customerId)
                                        .Single();

            if (customer == null)
            {
                return new List<Reservation>(); // Trả về danh sách rỗng nếu không có customer
            }*/

            var response = await _client.From<Reservation>()
                                        .Where(r => r.CustomerId == customerId)
                                        .Get();

            return response.Models;
        }

        public async Task<Reservation?> GetReservationById(Guid id)
        {
            return await _client.From<Reservation>().Where(r => r.Id == id).Single();
        }

        public async Task<ReservationResponseDTO> UpdateReservation(Guid id, ReservationRequestDTO request)
        {
            try
            {
                var reservation = await GetReservationById(id);
                if (reservation == null)
                {
                    return new ReservationResponseDTO
                    {
                        Success = false,
                        Message = "Reservation not found."
                    };
                }

                reservation.ReservationTime = request.ReservationTime;
                reservation.NumberOfGuests = request.NumberOfGuests;
                reservation.SpecialRequests = request.SpecialRequests;
                reservation.UpdatedAt = DateTime.UtcNow;

                await _client.From<Reservation>().Update(reservation);

                return new ReservationResponseDTO
                {
                    Success = true,
                    Message = "Reservation updated successfully.",
                    ReservationId = id
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<ReservationResponseDTO> DeleteReservation(Guid id)
        {
            try
            {
                var reservation = await GetReservationById(id);
                if (reservation == null)
                {
                    return new ReservationResponseDTO
                    {
                        Success = false,
                        Message = "Reservation not found."
                    };
                }

                await _client.From<Reservation>()
                    .Where(r => r.Id == id) 
                    .Delete();

                return new ReservationResponseDTO
                {
                    Success = true,
                    Message = "Reservation deleted successfully."
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

        public async Task<ReservationResponseDTO> CancelReservation(Guid id)
        {
            try
            {
                var reservation = await GetReservationById(id);
                if (reservation == null)
                {
                    return new ReservationResponseDTO
                    {
                        Success = false,
                        Message = "Reservation not found."
                    };
                }

                reservation.StatusEnum = ReservationStatus.Cancelled;
                await _client.From<Reservation>().Update(reservation);

                return new ReservationResponseDTO
                {
                    Success = true,
                    Message = "Reservation has been cancelled successfully."
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
            }
        }

    }
}

