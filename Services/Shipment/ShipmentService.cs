using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IShipmentSrvice{
    public Task<Shipment> CreateShipmentSrvice();
    public Task<bool> DeleteShipmentSrvice(Guid id);
    public Task<PaginatedResult<Shipment>> GetAllShipmentsService(QueryParameters queryParameters);
    public Task<ShipmentDto> GetShipmentByIdService(Guid id);
    public Task<ShipmentDto> UpdateShipmentSrvice(Guid id, ShipmentUpdateDto updateShipmentDto);
}
public class ShipmentService : IShipmentSrvice{
    readonly AppDbContext _appDbContext;
    readonly IMapper _mapper;
    public ShipmentService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<Shipment> CreateShipmentSrvice(){
        ShipmentCreateDto createShipmentDto = new ShipmentCreateDto();
        try
        {
        var mapShipment = _mapper.Map<Shipment>(createShipmentDto);
        mapShipment.ShipmentStatus = ShipmentStatus.OnProgress;
        await _appDbContext.Shipments.AddAsync(mapShipment);
        await _appDbContext.SaveChangesAsync();
            return mapShipment;
        }catch (Exception ex){
            throw new ApplicationException($"Error in create shipment service: {ex.Message}");
        }
    }
    public async Task<bool> DeleteShipmentSrvice(Guid id){
        try{
        var foundShipment = await _appDbContext.Shipments.FindAsync(id);
        if(foundShipment == null){
            return false;
        }else{
            _appDbContext.Shipments.Remove(foundShipment);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        }catch (Exception ex){
            throw new ApplicationException($"Error in delete shipment service: {ex.Message}");
        }
    }
    public async Task<PaginatedResult<Shipment>> GetAllShipmentsService(QueryParameters queryParameters){
        try
        {
            var query = _appDbContext.Shipments.AsQueryable();
            switch (queryParameters.SortBy?.ToLower()){
                case "OrderDate":
                    query = queryParameters.SortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(s => s.ShipmentDate)
                        : query.OrderBy(s => s.ShipmentDate);
                    break;
                default:
                    break;
            }
            var NumOfShipments = await query.CountAsync();
            if (queryParameters.PageNumber < 1) queryParameters.PageNumber = 1;
            if (queryParameters.PageSize < 1) queryParameters.PageSize = 5;
            if (NumOfShipments < 0)
            {
                Console.WriteLine("There is no shipments");
                return null;
            }
            var shipments = await query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize).ToListAsync();
            return new PaginatedResult<Shipment>
            {
                Items = shipments,
                TotalCount = NumOfShipments,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }catch (Exception ex){
            throw new ApplicationException($"Error in getting shipments service: {ex.Message}");
        }
    }
    public async Task<ShipmentDto> GetShipmentByIdService(Guid id){
        try{
        var shipment = await _appDbContext.Shipments.FindAsync(id);
        if (shipment == null) return null;
        return _mapper.Map<ShipmentDto>(shipment);
        }catch (Exception ex){
            throw new ApplicationException($"Error in getting a shipment by id service: {ex.Message}");
        }
    }
    public async Task<ShipmentDto> UpdateShipmentSrvice(Guid id, ShipmentUpdateDto updateShipmentDto){
        if (updateShipmentDto == null){
            throw new ArgumentNullException(nameof(updateShipmentDto), "Update data must be provided.");
        }
        
        try{
        var shipment = await _appDbContext.Shipments.FindAsync(id);      
        if(shipment == null){
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }
        _mapper.Map(updateShipmentDto, shipment);
        _appDbContext.Shipments.Update(shipment);
        await _appDbContext.SaveChangesAsync();
        
        var mappingShipment = _mapper.Map<ShipmentDto>(shipment);
        return mappingShipment;
        }catch (Exception ex){
            throw new ApplicationException($"Error in updating a shipment service: {ex.Message}");
        }
    }
}