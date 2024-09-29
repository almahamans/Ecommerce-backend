using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/orders")]
public class OrderController : ControllerBase{
    readonly OrderService _orderService;
    public OrderController(OrderService orderService){
        _orderService = orderService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto){
        var newOrder = _orderService.CreateOrderSrvice(createOrderDto);
        Console.WriteLine("---------constroller");
        return Created("A new Order created successfully", newOrder);
    }
    [HttpGet]
    public async Task<IActionResult> GetOrders(){
        var orders = _orderService.GetAllOrdersService();
        return Ok(orders);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id){
        if(id == null){
            return NotFound();
        }
        var order = _orderService.GetOrderByIdService(id);
        return Ok(order);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrderById(Guid id){
        if (id == null)
        {
            return NotFound();
        }
        var order = _orderService.DeleteOrderSrvice(id);
        return Ok(order);
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOrderById(Guid id, [FromBody] UpdateOrderDto updateOrderDto){
        if (id == null)
        {
            return NotFound();
        }
        var order = _orderService.UpdateOrderStatusSrvice(id, updateOrderDto);
        return Ok(order);
    }
}