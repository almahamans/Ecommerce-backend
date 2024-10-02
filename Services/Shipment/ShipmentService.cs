using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IShipmentSrvice{
    public Task<Shipment> CreateShipmentSrvice(CreateShipmentDto createShipmentDto);
    public Task<bool> DeleteShipmentSrvice(Guid id);
    public Task<List<Shipment>> GetAllShipmentsService();
    public Task<ShipmentDto> GetShipmentByIdService(Guid id);
    public Task<ShipmentDto> UpdateShipmentSrvice(Guid id, UpdateShipmentDto updateShipmentDto);
}

public class ShipmentService : IShipmentSrvice{
    AppDbContext _appDbContext;
    IMapper _mapper;
    public ShipmentService(AppDbContext appDbContext, IMapper mapper){
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<Shipment> CreateShipmentSrvice(CreateShipmentDto createShipmentDto){
        if(createShipmentDto == null){
            return null;
        }
        var mapShipment = _mapper.Map<Shipment>(createShipmentDto);
        mapShipment.ShipmentStatus = ShipmentStatus.OnProgress;
        await _appDbContext.Shipments.AddAsync(mapShipment);
        await _appDbContext.SaveChangesAsync();
        return mapShipment;
    }
    public async Task<bool> DeleteShipmentSrvice(Guid id){
        var foundShipment = await _appDbContext.Shipments.FindAsync(id);
       if(foundShipment == null){
        return false;
       }else{
        _appDbContext.Shipments.Remove(foundShipment);
        await _appDbContext.SaveChangesAsync();
        return true;
       }
    }
    public async Task<List<Shipment>> GetAllShipmentsService(){
        var shipment = await _appDbContext.Shipments.ToListAsync();
        if (shipment.Count() < 0){
            Console.WriteLine("Empty List");
            return null;
        }else{
            return shipment;
        }
    }
    public async Task<ShipmentDto> GetShipmentByIdService(Guid id){
        Console.WriteLine("-----------------service");
        var shipment = await _appDbContext.Shipments.FindAsync(id);
        if (shipment == null) return null;
        return _mapper.Map<ShipmentDto>(shipment);
    }
    public async Task<ShipmentDto> UpdateShipmentSrvice(Guid id, UpdateShipmentDto updateShipmentDto){
        var shipment = await _appDbContext.Shipments.FindAsync(id);
        
        if (shipment == null) return null;
        if (updateShipmentDto == null) return null;

        shipment.ShipmentStatus = updateShipmentDto.ShipmentStatus ?? shipment.ShipmentStatus;
        shipment.ShipmentDate = updateShipmentDto.ShipmentDate ?? shipment.ShipmentDate;
        shipment.DeliveryDate = updateShipmentDto.DeliveryDate ?? shipment.DeliveryDate;
        _appDbContext.Shipments.Update(shipment);
        await _appDbContext.SaveChangesAsync();
        var mappingShipment = _mapper.Map<ShipmentDto>(shipment);
        return mappingShipment;
    }
}