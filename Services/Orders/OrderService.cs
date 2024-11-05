using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IOrderService
{
    public Task<Order> CreateOrderSrvice(OrderCreateDto createOrderDto);
    public Task<bool> DeleteOrderSrvice(Guid id);
    public Task<PaginatedResult<Order>> GetAllOrdersService(QueryParameters queryParameters);
    public Task<OrderDto> GetOrderByIdService(Guid id);
    public Task<OrderDto> UpdateOrderSrvice(Guid id, OrderUpdateDto updateOrderDto);
}
public class OrderService : IOrderService
{
    readonly AppDbContext _appDbContext;
    readonly IMapper _mapper;

    public OrderService(AppDbContext appDbContext, IMapper mapper){
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<Order> CreateOrderSrvice(OrderCreateDto createOrderDto)
    {
        try
        {
        if (createOrderDto == null){
            return null;
        }else{
            var newOrder = _mapper.Map<Order>(createOrderDto);
            newOrder.UserId = createOrderDto.UserId;
            await _appDbContext.Orders.AddAsync(newOrder);
            await _appDbContext.SaveChangesAsync();

        // foreach (var orderproduct in createOrderDto.OrderProducts){
        // var newOrderProduct = new OrderProduct{
        //     ProductQuantity = orderproduct.ProductQuantity,
        //     ProductsPrice = orderproduct.ProductsPrice,
        //     OrderId = newOrder.OrderId
        // };
        //     await _appDbContext.OrderProducts.AddAsync(newOrderProduct);     
        // }
        //     await _appDbContext.SaveChangesAsync();

            var newShipment = new Shipment{
            ShipmentStatus = ShipmentStatus.OnProgress,
            OrderId = newOrder.OrderId
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
            var order = await _appDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

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

            switch (queryParameters.sortBy?.ToLower()){
                case "OrderDate":
                    query = queryParameters.sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(o => o.OrderDate)
                        : query.OrderBy(o => o.OrderDate);
                    break;
                default:
                    break;
            }

            var NumOforders = await query.CountAsync();
            if (queryParameters.pageNumber < 1) queryParameters.pageNumber = 1;
            if (queryParameters.pageSize < 1) queryParameters.pageSize = 5;
            if (NumOforders < 0) return null;
            
            var orders = await query.Skip((queryParameters.pageNumber - 1) * queryParameters.pageSize).Take(queryParameters.pageSize).ToListAsync();
            return new PaginatedResult<Order>
            {
                Items = orders,
                TotalCount = NumOforders,
                PageNumber = queryParameters.pageNumber,
                PageSize = queryParameters.pageSize
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
    public async Task<OrderDto> UpdateOrderSrvice(Guid id, OrderUpdateDto updateOrderDto){
        if (updateOrderDto == null){
            throw new ArgumentNullException(nameof(updateOrderDto), "Update data must be provided.");
        }
        try{
            var order = await _appDbContext.Orders.FindAsync(id);
            if (order == null){
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }
            _mapper.Map(updateOrderDto, order);
            _appDbContext.Orders.Update(order);
            await _appDbContext.SaveChangesAsync();
//to return result in orderDto
            var mappingOrder = _mapper.Map<OrderDto>(order);
            return mappingOrder;
        }catch (Exception ex){
            throw new ApplicationException($"Error in updating an order service: {ex.Message}");
        }
    }
}