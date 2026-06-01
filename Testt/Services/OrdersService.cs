using Microsoft.EntityFrameworkCore;
using Testt.Data;
using Testt.DTOs;
using Testt.Exceptions;

namespace Testt.Services;

public class OrdersService : IOrdersService
{
    private readonly DatabaseContext _context;

    public OrdersService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                FulfilledAt = o.FulfilledAt,
                Status = o.Status.Name,
                Client = new ClientDto
                {
                    FirstName = o.Client.FirstName,
                    LastName = o.Client.LastName
                },
                Products = o.ProductOrders.Select(po => new ProductDto
                {
                    Name = po.Product.Name,
                    Price = po.Product.Price,
                    Amount = po.Amount
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (order is null)
        {
            throw new NotFoundException("Order not found");
        }

        return order;
    }

    public async Task FulfillOrderAsync(int id, FulfillOrderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.StatusName))
        {
            throw new ConflictException("Status name is required");
        }

        var order = await _context.Orders
            .Include(o => o.ProductOrders)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
        {
            throw new NotFoundException("Order not found");
        }

        if (order.FulfilledAt is not null)
        {
            throw new ConflictException("Order is already completed");
        }

        var status = await _context.Statuses
            .FirstOrDefaultAsync(s => s.Name == dto.StatusName);

        if (status is null)
        {
            throw new NotFoundException("Status not found");
        }

        order.StatusId = status.Id;
        order.FulfilledAt = DateTime.Now;

        _context.ProductOrders.RemoveRange(order.ProductOrders);

        await _context.SaveChangesAsync();
    }
}
