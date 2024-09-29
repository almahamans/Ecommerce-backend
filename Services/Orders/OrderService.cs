using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class OrderService{
    readonly AppDbContext _appDbContext;
    readonly IMapper _mapper;
    public OrderService(AppDbContext appDbContext, IMapper mapper){
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<Order> CreateOrderSrvice(CreateOrderDto createOrderDto){
        Console.WriteLine("---------service");
        var order = _mapper.Map<Order>(createOrderDto);
        await _appDbContext.Orders.AddAsync(order);
        await _appDbContext.SaveChangesAsync();
        return order;
    }
    public async Task<bool> DeleteOrderSrvice(Guid id){
        var order = await _appDbContext.Orders.FindAsync(id);
        if(order == null){
            return false;
        }
        _appDbContext.Orders.Remove(order);
        await _appDbContext.SaveChangesAsync();
        return true;
    }
    public async Task<List<OrderDto>> GetAllOrdersService(){
        var order = await _appDbContext.Orders.ToListAsync();
        var orderData = _mapper.Map<List<OrderDto>>(order);
        return orderData;
    }
    public async Task<OrderDto> GetOrderByIdService(Guid id){
        var order = await _appDbContext.Orders.FindAsync(id);
        if (order == null)
        {
            return null;
        }
        return _mapper.Map<OrderDto>(order);
    }
    public async Task<OrderDto> UpdateOrderStatusSrvice(Guid id, UpdateOrderDto updateOrderDto){
        var order = await _appDbContext.Orders.FindAsync(id);
        if (order == null)
        {
            return null;
        }
        order.OrderStatus = updateOrderDto.orderStatus ?? order.OrderStatus;
        _appDbContext.Orders.Update(order);
        await _appDbContext.SaveChangesAsync();
        var mappingOrder = _mapper.Map<OrderDto>(order);
        return mappingOrder;
    }
}