using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
[ApiController, Route("api/v1/orders")]
public class OrderController : ControllerBase{
    readonly IOrderService _iorderService;
    public OrderController(IOrderService iorderService )
    {
        _iorderService = iorderService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto createOrderDto){      
        if(!ModelState.IsValid){
            return ApiResponse.BadRequest();
        }
        try{
            var newOrder = await _iorderService.CreateOrderSrvice(createOrderDto);
            return ApiResponse.Created(newOrder, "A new Order created successfully");
        }catch(Exception ex){
           return ApiResponse.NotFound($"error in creating the order (controller). {ex.Message}");
        }  
    }      
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] QueryParameters queryParameters)
    {
        try{
            var orders = await _iorderService.GetAllOrdersService(queryParameters);
            return ApiResponse.Success(orders);
        }catch(Exception ex){
            return ApiResponse.NotFound($"error in geting orders (controller). {ex.Message}");
        }        
    }

    [Authorize(Roles = "Admin, Customer")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id){
        if(id == null){
            return ApiResponse.BadRequest("no id found");
        }
        try{
        var order = await _iorderService.GetOrderByIdService(id);
        return ApiResponse.Success(order);
        }catch(Exception ex){
            return ApiResponse.NotFound($"error in geting the order (controller). {ex.Message}");
        }
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrderById(Guid id){
        if (id == null){
            return ApiResponse.BadRequest("No id found");
        }
        try{
            var order = await _iorderService.DeleteOrderSrvice(id);
            return ApiResponse.Success(order);  
        }catch(Exception ex){
            return ApiResponse.NotFound($"error in deleting order (controller).{ex.Message}");
        }
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOrderById(Guid id, OrderUpdateDto updateOrderDto){
        if (updateOrderDto == null){
            return ApiResponse.BadRequest("Update data must be provided.");
        }
        try{
            var order = await _iorderService.UpdateOrderSrvice(id, updateOrderDto);
            if(order == null){
                return ApiResponse.NotFound("No id fount");
            }
            return ApiResponse.Success(order);  
        }catch (Exception ex){
            return ApiResponse.NotFound($"No entered data. {ex.Message}");
        }
    }
}