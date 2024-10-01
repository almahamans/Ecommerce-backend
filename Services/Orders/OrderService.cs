using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IOrderService{
    public Task<Order> CreateOrderSrvice(CreateOrderDto createOrderDto);
    public Task<bool> DeleteOrderSrvice(Guid id);
    public Task<PaginatedResult<Order>> GetAllOrdersService(int pageNumber, int pageSize);
    public Task<OrderDto> GetOrderByIdService(Guid id);
    public Task<OrderDto> UpdateOrderStatusSrvice(Guid id, UpdateOrderDto updateOrderDto);
}
public class OrderService : IOrderService{
    readonly AppDbContext _appDbContext;
    readonly IMapper _mapper;
    public OrderService(AppDbContext appDbContext, IMapper mapper){
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<Order> CreateOrderSrvice(CreateOrderDto createOrderDto){
        try{
        if(createOrderDto == null){
            return null;
        }else{
            var order =  _mapper.Map<Order>(createOrderDto);
            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();
            return order;  
        }
        }catch (Exception ex){
            throw new ApplicationException(ex.Message);
        }
    }
    public async Task<bool> DeleteOrderSrvice(Guid id){
        try{
        var order = await _appDbContext.Orders.FindAsync(id);
        if(order == null){
            return false;
        }else{
            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();
            return true;  
        }
        }catch (Exception ex){
            throw new ApplicationException(ex.Message);
        }
    }
    public async Task<PaginatedResult<Order>> GetAllOrdersService(int pageNumber, int pageSize){
        try{
        var NumOforders = await _appDbContext.Orders.CountAsync();
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;
        if (NumOforders < 0){
            Console.WriteLine("There is no orders");
            return null;
        }
        var orders = await _appDbContext.Orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); 
        return new PaginatedResult<Order>
        {
            Items = orders,
            TotalCount = NumOforders,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        }catch (Exception ex){
            throw new ApplicationException(ex.Message);
        }
    }
    public async Task<OrderDto> GetOrderByIdService(Guid id){
        var order = await _appDbContext.Orders.FindAsync(id);
        if (order == null){
            return null;
        }
        return _mapper.Map<OrderDto>(order);
    }
    public async Task<OrderDto> UpdateOrderStatusSrvice(Guid id, UpdateOrderDto updateOrderDto){
        try{
        var order = await _appDbContext.Orders.FindAsync(id);
        if (order == null){
            return null;
        }
        if(updateOrderDto == null){
            return null;
        }
        order.TotalAmount = updateOrderDto.TotalAmount ?? order.TotalAmount;
        _appDbContext.Orders.Update(order);
        await _appDbContext.SaveChangesAsync();
        var mappingOrder = _mapper.Map<OrderDto>(order);
        return mappingOrder;
        }catch (Exception ex){
            throw new ApplicationException(ex.Message);
        }
    }
}