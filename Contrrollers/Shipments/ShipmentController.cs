using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/v1/shipments")]
public class ShipmentController : ControllerBase{
    readonly IShipmentSrvice _ishipmentSrvice;
    public ShipmentController(IShipmentSrvice ishipmentSrvice){
        _ishipmentSrvice = ishipmentSrvice;
    }
    [HttpPost]
    public async Task<IActionResult> CrerateShipment(CreateShipmentDto createShipmentDto){
        if (!ModelState.IsValid){
            return ApiResponse.BadRequest();
        }
        try{
        var newShipment = await _ishipmentSrvice.CreateShipmentSrvice(createShipmentDto);
        return ApiResponse.Created(newShipment, "A new shipment created");
        }catch(Exception ex){
            return ApiResponse.NotFound($"Error in create shipment controller: {ex.Message}");
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllShipments(){
        if (!ModelState.IsValid){
            return ApiResponse.BadRequest();
        }
        try{
        var shipments = await _ishipmentSrvice.GetAllShipmentsService();
        if(shipments.Count() < 0) return ApiResponse.NotFound("Shipping list is empty");
        return ApiResponse.Success(shipments);
        }catch(Exception ex){
            return ApiResponse.NotFound($"Error in Getting shipments controller: {ex.Message}");
        }
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteShipment(Guid id){
        if (!ModelState.IsValid){
            return ApiResponse.BadRequest();
        }
        try{
        var foundShipment = await _ishipmentSrvice.DeleteShipmentSrvice(id);
        return ApiResponse.Success(foundShipment, "Shipment deleted successfully");
        }catch (Exception ex){
            return ApiResponse.NotFound($"Error in deleting shipment controller: {ex.Message}");
        }
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetShipmentById(Guid id){
        if (!ModelState.IsValid){
            return ApiResponse.BadRequest();
        }
        try{
            var foundShipment = await _ishipmentSrvice.GetShipmentByIdService(id);
            return ApiResponse.Success(foundShipment);
        }catch (Exception ex){
            return ApiResponse.NotFound($"Error in finding a shipment controller: {ex.Message}");
        }
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateShipmentInfo(Guid id, UpdateShipmentDto updateShipmentDto){
        if (!ModelState.IsValid){
            return ApiResponse.BadRequest();
        }
        try{
            var shipment = await _ishipmentSrvice.UpdateShipmentSrvice(id, updateShipmentDto);
            return Ok(shipment);
        }catch (Exception ex){
            return ApiResponse.NotFound($"Not entered data. {ex.Message}");
        }
    }
}