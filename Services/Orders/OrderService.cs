using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IOrderService{
    public Task<Order> CreateOrderSrvice(CreateOrderDto createOrderDto);
    public Task<bool> DeleteOrderSrvice(Guid id);
    public Task<PaginatedResult<Order>> GetAllOrdersService(QueryParameters queryParameters);
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
            Shipment shipment = new Shipment();
               
            // when you create an order 
            // create one object instance of Shipment 
            // then assign the order id into the shipment.OrderId
            await _appDbContext.Orders.Include(s => s.Shipment).FirstOrDefaultAsync(o => o.OrderId == shipment.OrderId);
            
            var order =  _mapper.Map<Order>(createOrderDto);
            var ordershipment = order.Shipment.ShipmentId; 
            order.ShipmentId = ordershipment;
            _mapper.Map<Order> order.Shipment.ShipmentStatus;
            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();
            // var odrerStatus = order.Shipment.ShipmentStatus;
            // var maporderstatus = _mapper.Map<Order>(odrerStatus);
            // Console.WriteLine($"--------------------------{maporderstatus}");
            return  order;  
        }
        }catch (Exception ex){
            throw new ApplicationException($"Error in create order service: {ex.Message}");
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
        }
        catch (Exception ex){
            throw new ApplicationException($"Error in delete order service: {ex.Message}");
        }
    }
    public async Task<PaginatedResult<Order>> GetAllOrdersService(QueryParameters queryParameters){
        try{
            var query = _appDbContext.Orders.AsQueryable();
            switch (queryParameters.SortBy?.ToLower())
            {
                case "OrderDate":
                    query = queryParameters.SortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(o => o.OrderDate)
                        : query.OrderBy(o => o.OrderDate);
                    break;
                default:
                    break;
            }
        var NumOforders = await query.CountAsync();
        if (queryParameters.PageNumber < 1) queryParameters.PageNumber = 1;
        if (queryParameters.PageSize < 1) queryParameters.PageSize = 5;
        if (NumOforders < 0){
            Console.WriteLine("There is no orders");
            return null;
        }
        var orders = await query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize).ToListAsync();
        // orders = _mapper.Map<List<Order>>(query.Select(s => s.Shipment.ShipmentStatus).ToList());
        // var shipmentStat = _appDbContext.Shipments.Select(x => x.shipmentId != null && _appDbContext.Orders.FirstOrDefaultAsync(x => x.ShipmentId != null)).ShipmentStatus;
        // var shipmentOfOrder = orders.Find(x => x.OrderId == shipmentId);
        // var shipmentOfOrder = await _appDbContext.Orders.Include(o => o.Shipment).FirstOrDefaultAsync(x => x.OrderId == );
            
            return new PaginatedResult<Order>
        {
            Items = orders,
            TotalCount = NumOforders,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize
        };
        }catch (Exception ex){
            throw new ApplicationException($"Error in getting orders service: {ex.Message}");
        }
    }
    public async Task<OrderDto> GetOrderByIdService(Guid id){
        try{
        var order = await _appDbContext.Orders.FindAsync(id);
        if (order == null){
            return null;
        }
        return _mapper.Map<OrderDto>(order);
        }catch (Exception ex){
            throw new ApplicationException($"Error in getting an order by id service: {ex.Message}");
        }
    }
    public async Task<OrderDto> UpdateOrderStatusSrvice(Guid id, UpdateOrderDto updateOrderDto){
        try{
        var order = await _appDbContext.Orders.FindAsync(id);
        
        if (order == null) return null;
        if(updateOrderDto == null) return null;

        order.TotalAmount = updateOrderDto.TotalAmount ?? order.TotalAmount;
        _appDbContext.Orders.Update(order);
        await _appDbContext.SaveChangesAsync();
        var mappingOrder = _mapper.Map<OrderDto>(order);
        return mappingOrder;
        }catch (Exception ex){
            throw new ApplicationException($"Error in updating an order service: {ex.Message}");
        }
    }
}