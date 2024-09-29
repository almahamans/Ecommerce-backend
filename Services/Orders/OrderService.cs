using AutoMapper;

public interface IOrderService{
    List<OrderDto> GetAllOrdersService();
    OrderDto GetOrderByIdService(Guid id);
    bool CreateOrderSrvice(CreateOrderDto createOrderDto);
    bool DeleteOrderSrvice(Guid id);
    bool UpdateOrderStatusSrvice(Guid id);
}

public class OrderService : IOrderService{
    static readonly List<Order> _orders = new List<Order>();
    readonly AppDbContext _appDbContext;
    readonly IMapper _mapper;
    public OrderService(AppDbContext appDbContext, IMapper mapper){
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public bool CreateOrderSrvice(CreateOrderDto createOrderDto){
        throw new NotImplementedException();
    }

    public bool DeleteOrderSrvice(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<OrderDto> GetAllOrdersService()
    {
        throw new NotImplementedException();
    }

    public OrderDto GetOrderByIdService(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool UpdateOrderStatusSrvice(Guid id)
    {
        throw new NotImplementedException();
    }
}