using Microsoft.AspNetCore.Mvc;
 
[ApiController, Route("/api/v1/orders")]
public class OrderController : ControllerBase{
    readonly IOrderService _iorderService;
    public OrderController(IOrderService iorderService){
        _iorderService = iorderService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto){      
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
    public async Task<IActionResult> GetOrders([FromBody] int pageNumber = 1, int pageSize = 5){
        try{
            var orders = await _iorderService.GetAllOrdersService(pageNumber, pageSize);
            return ApiResponse.Success(orders);
        }catch(Exception ex){
            return ApiResponse.NotFound($"error in geting orders (controller). {ex.Message}");
        }        
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(Guid id){
        // if(id == null){
        //     return ApiResponse.BadRequest("no id found");
        // }
        try{
        var order = await _iorderService.GetOrderByIdService(id);
        return ApiResponse.Success(order);
        }catch(Exception ex){
            return ApiResponse.NotFound($"error in geting the order (controller). {ex.Message}");
        }
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrderById(Guid id){
        // if (id == ){
        //     return ApiResponse.BadRequest("No id found");
        // }
        try{
            var order = await _iorderService.DeleteOrderSrvice(id);
            return ApiResponse.Success(order);  
        }catch(Exception ex){
            return ApiResponse.NotFound($"error in deleting order (controller).{ex.Message}");
        }
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOrderById(Guid id, [FromBody] UpdateOrderDto updateOrderDto){
        // if (id == null){
        //     return ApiResponse.BadRequest("no id found");
        // }
        try{
            var order = await _iorderService.UpdateOrderStatusSrvice(id, updateOrderDto);
            return Ok(order);  
        }catch (Exception ex){
            return ApiResponse.NotFound($"Not entered data. {ex.Message}");
        }
    }
}