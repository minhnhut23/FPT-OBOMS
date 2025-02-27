using BusinessObject.DTOs.ShopDTO;
using BusinessObject.DTOs.SubscriptionDTO;
using BusinessObject.Models;
using BusinessObject.Utils;
using Supabase;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ShopManagementService.DAO;

public class SubscriptionDAO
{
    private readonly Client _client;

    public SubscriptionDAO(Client client)
    {
        _client = client;
    }

    public async Task<List<SubscriptionResponseDTO>> GetAll()
    {
        try
        {
            var subs = await _client.From<Subscriptions>().Get();
            var response = subs.Models;

            return response.Select(sub => new SubscriptionResponseDTO
            {
                Id = sub.Id,
                Name = sub.Name,
                Description = sub.Description,
                Price = sub.Price,
                NumberOfMonths = sub.NumberOfMonths
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task Create(CreateSubscriptionRequestDTO request)
    {
        try
        {
            var sub = new Subscriptions
            {
                Name = request.Name,
                Price = request.Price,
                NumberOfMonths = request.NumberOfMonths,
                Description = request.Description!,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            var response = await _client.From<Subscriptions>().Insert(sub);

        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task<SubscriptionResponseDTO> GetById(Guid requestId)
    {
        try
        {
            var sub = await _client.From<Subscriptions>().Where(s => s.Id == requestId).Single();

            if (sub == null)
            {
                throw new Exception("Id not found.");
            }

            return new SubscriptionResponseDTO
            {
                Id = sub.Id,
                Name = sub.Name,
                Description = sub.Description,
                Price = sub.Price,
                NumberOfMonths = sub.NumberOfMonths
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

    public async Task<SubscriptionResponseDTO> Update(Guid requestId, UpdateSubscriptionRequestDTO request)
    {
        try
        {
            var sub = await _client.From<Subscriptions>().Where(s => s.Id == requestId).Single();

            if (sub == null)
            {
                throw new Exception("Id not found.");
            }

            sub.Name = request.Name ?? sub.Name;
            sub.Description = request.Description ?? sub.Description;
            sub.Price = request.Price ?? sub.Price;
            sub.NumberOfMonths = request.NumberOfMonths ?? sub.NumberOfMonths;

            var response = await _client.From<Subscriptions>().Update(sub);
            if (response == null) throw new Exception("Error updating subscription.");

            return new SubscriptionResponseDTO
            {
                Id = sub.Id,
                Name = sub.Name,
                Description = sub.Description,
                Price = sub.Price,
                NumberOfMonths = sub.NumberOfMonths
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ErrorHandler.ProcessErrorMessage(ex.Message));
        }
    }

}
