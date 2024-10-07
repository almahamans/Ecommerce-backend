using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IOrderService
{
    public Task<Order> CreateOrderSrvice(CreateOrderDto createOrderDto);
    public Task<bool> DeleteOrderSrvice(Guid id);
    public Task<PaginatedResult<Order>> GetAllOrdersService(QueryParameters queryParameters);
    public Task<OrderDto> GetOrderByIdService(Guid id);
    public Task<OrderDto> UpdateOrderSrvice(Guid id, UpdateOrderDto updateOrderDto);
}
public class OrderService : IOrderService
{
    readonly AppDbContext _appDbContext;
    readonly IMapper _mapper;

    public OrderService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<Order> CreateOrderSrvice(CreateOrderDto createOrderDto)
    {
        try
        {
        if (createOrderDto == null){
            return null;
        }else{
            var newOrder = _mapper.Map<Order>(createOrderDto);
            await _appDbContext.Orders.AddAsync(newOrder);
            newOrder.UserId = createOrderDto.UserId;
            await _appDbContext.SaveChangesAsync();

        foreach (var orderproduct in createOrderDto.OrderProducts){
        var newOrderProduct = new OrderProduct{
            ProductQuantity = orderproduct.ProductQuantity,
            ProductsPrice = orderproduct.ProductsPrice,
            OrderId = orderproduct.OrderId
        };
            await _appDbContext.OrderProducts.AddAsync(newOrderProduct);     
        }
            await _appDbContext.SaveChangesAsync();

            var newShipment = new Shipment{
            ShipmentStatus = ShipmentStatus.OnProgress,
            OrderId = newOrder.OrderId,
            Order = newOrder
        };
            await _appDbContext.Shipments.AddAsync(newShipment);
            await _appDbContext.SaveChangesAsync();
            newOrder.ShipmentId = newShipment.ShipmentId;
            await _appDbContext.SaveChangesAsync();

            return newOrder;  
        }
        }catch (Exception ex){
            throw new ApplicationException($"Error in create order service: {ex.Message}");
        }
    }
    public async Task<bool> DeleteOrderSrvice(Guid id){
        try{
            var order = await _appDbContext.Orders
            .Include(o => o.Shipment)
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return false;

            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();
            
            return true;
        }catch (Exception ex){
            throw new ApplicationException($"Error in delete order service: {ex.Message}");
        }
    }

    public async Task<PaginatedResult<Order>> GetAllOrdersService(QueryParameters queryParameters){
        try{
            var query = _appDbContext.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.User)
            .AsQueryable();

            switch (queryParameters.SortBy?.ToLower()){
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
            if (NumOforders < 0) return null;
            
            var orders = await query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize).ToListAsync();
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
            var order = await _appDbContext.Orders
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return null;

            return _mapper.Map<OrderDto>(order);
        }catch (Exception ex){
            throw new ApplicationException($"Error in getting an order by id service: {ex.Message}");
        }
    }
    public async Task<OrderDto> UpdateOrderSrvice(Guid id, UpdateOrderDto updateOrderDto){
        try
        {
            var order = await _appDbContext.Orders.FindAsync(id);

            if (order == null) return null;
            if (updateOrderDto == null) return null;
//need to include orderProduct also <future work?>
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