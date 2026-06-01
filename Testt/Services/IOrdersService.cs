using Testt.DTOs;

namespace Testt.Services;

public interface IOrdersService
{
    Task<OrderDto> GetOrderByIdAsync(int id);
    Task FulfillOrderAsync(int id, FulfillOrderDto dto);
}
