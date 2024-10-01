using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IOrderService{
    public Task<Order> CreateOrderSrvice(CreateOrderDto createOrderDto);
    public Task<bool> DeleteOrderSrvice(Guid id);
    public Task<List<OrderDto>> GetAllOrdersService();
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
        if(createOrderDto == null){
            return null;
        }else{
            var order =  _mapper.Map<Order>(createOrderDto);
            order.OrderStatus = OrderStatus.OnProgress;
            // // //maybe in the frontend can print the value???????????
            Console.WriteLine(order.OrderStatus);
            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();
            return order;  
        }
    }
    public async Task<bool> DeleteOrderSrvice(Guid id){
        var order = await _appDbContext.Orders.FindAsync(id);
        if(order == null){
            return false;
        }else{
            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();
            return true;  
        }
    }
    public async Task<List<OrderDto>> GetAllOrdersService(){
        var order = await _appDbContext.Orders.ToListAsync();
        if(order.Count() < 0){
            Console.WriteLine("Empty List");
            return null; 
        }else{
            var orderData = _mapper.Map<List<OrderDto>>(order);
            return orderData;    
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
        var order = await _appDbContext.Orders.FindAsync(id);
        if (order == null){
            return null;
        }
        if(updateOrderDto == null){
            return null;
        }
        order.OrderStatus = updateOrderDto.orderStatus ?? order.OrderStatus;
        order.TotalAmount = updateOrderDto.TotalAmount ?? order.TotalAmount;
        order.Quantity = updateOrderDto.Quantity ?? order.Quantity;
        _appDbContext.Orders.Update(order);
        await _appDbContext.SaveChangesAsync();
        var mappingOrder = _mapper.Map<OrderDto>(order);
        return mappingOrder;
    }
}